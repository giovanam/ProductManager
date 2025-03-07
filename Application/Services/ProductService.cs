using MassTransit;
using ProductManager.Application.DTOs;
using ProductManager.Contracts.Events;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;

namespace ProductManager.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, IPublishEndpoint publishEndpoint, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task AddProductAsync(ProductDTO productDTO)
        {
            try
            {
                var product = new Product
                {
                    Nome = productDTO.Nome,
                    Preco = productDTO.Preco,
                    QuantidadeEstoque = productDTO.QuantidadeEstoque
                };

                await _productRepository.AddProductAsync(product);
                _logger.LogInformation("Produto adicionado com sucesso: {ProductName}", product.Nome);

                await _publishEndpoint.Publish(new ProductCreatedEvent(product.Id, product.Nome, product.Preco, product.QuantidadeEstoque));
                _logger.LogInformation("Evento ProductCreated publicado para o produto: {ProductName}", product.Nome);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar produto: {ProductName}", productDTO.Nome);
                throw;
            }
        }

        public async Task UpdateProductAsync(ProductDTO productDTO)
        {
            try
            {
                var existingProduct = await _productRepository.GetByIdAsync(productDTO.Id);
                if (existingProduct == null)
                {
                    _logger.LogWarning("Produto não encontrado para atualização. ProductId = {ProductId}", productDTO.Id);
                    throw new Exception("Produto não encontrado.");
                }

                existingProduct.Nome = productDTO.Nome;
                existingProduct.Preco = productDTO.Preco;
                existingProduct.QuantidadeEstoque = productDTO.QuantidadeEstoque;

                await _productRepository.UpdateProductAsync(existingProduct);
                _logger.LogInformation("Produto atualizado com sucesso: {ProductName}", existingProduct.Nome);

                await _publishEndpoint.Publish(new ProductUpdatedEvent(existingProduct.Id, existingProduct.Nome, existingProduct.Preco, existingProduct.QuantidadeEstoque));
                _logger.LogInformation("Evento ProductUpdated publicado para o produto: {ProductName}", existingProduct.Nome);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar produto: {ProductName}", productDTO.Nome);
                throw;
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var existingProduct = await _productRepository.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    _logger.LogWarning("Produto não encontrado para remoção. ProductId = {ProductId}", id);
                    throw new Exception("Produto não encontrado.");
                }

                await _productRepository.DeleteProductAsync(id);
                _logger.LogInformation("Produto removido com sucesso: {ProductId}", id);

                await _publishEndpoint.Publish(new ProductDeletedEvent(id));
                _logger.LogInformation("Evento ProductDeleted publicado para o produto: {ProductId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover produto: {ProductId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                _logger.LogInformation("Produtos recuperados com sucesso: {ProductCount}", products.Count());
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar todos os produtos.");
                throw;
            }
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Produto não encontrado. ProductId = {ProductId}", id);
                    throw new Exception("Produto não encontrado.");
                }

                _logger.LogInformation("Produto recuperado com sucesso: {ProductName}", product.Nome);
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar produto: {ProductId}", id);
                throw;
            }
        }
    }
}
