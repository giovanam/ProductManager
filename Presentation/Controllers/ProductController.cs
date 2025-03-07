using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.DTOs;
using ProductManager.Domain.Interfaces;

namespace ProductManager.Presentation.Controllers
{
    [ApiController]
    [Route("api/produtos")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService,
                                 ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                if (products == null || !products.Any())
                {
                    _logger.LogInformation("Nenhum produto encontrado.");
                    return NotFound("Nenhum produto encontrado.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter produtos.");
                return StatusCode(500, "Erro ao processar a solicitação.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProductAsync([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null)
            {
                return BadRequest("Os dados do produto são inválidos.");
            }

            try
            {
                await _productService.AddProductAsync(productDTO);
                _logger.LogInformation("Produto adicionado com sucesso: {ProductName}", productDTO.Nome);
                return CreatedAtAction(nameof(GetAllAsync), new { id = productDTO.Id }, productDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar produto.");
                return StatusCode(500, "Erro ao processar a solicitação.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductAsync([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null)
            {
                return BadRequest("Os dados do produto são inválidos.");
            }

            try
            {
                await _productService.UpdateProductAsync(productDTO);
                _logger.LogInformation("Produto atualizado com sucesso: {ProductName}", productDTO.Nome);
                return NoContent(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar produto.");
                return StatusCode(500, "Erro ao processar a solicitação.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Produto não encontrado para exclusão. ProductId = {ProductId}", id);
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                await _productService.DeleteProductAsync(id);
                _logger.LogInformation("Produto excluído com sucesso. ProductId = {ProductId}", id);
                return Ok($"Produto com ID {id} excluído com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir produto.");
                return StatusCode(500, "Erro ao processar a solicitação.");
            }
        }
    }
}
