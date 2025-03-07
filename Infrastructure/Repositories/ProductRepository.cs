using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;
using ProductManager.Infrastructure.Data;

namespace ProductManager.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductManagerContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(ProductManagerContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Produto adicionado com sucesso: {ProductName}", product.Nome);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar produto: {ProductName}", product.Nome);
                throw;
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Produto atualizado com sucesso: {ProductName}", product.Nome);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar produto: {ProductName}", product.Nome);
                throw;
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var product = await GetByIdAsync(id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Produto removido com sucesso: {ProductId}", id);
                }
                else
                {
                    _logger.LogWarning("Produto não encontrado para remoção. ProductId = {ProductId}", id);
                }
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
                var products = await _context.Products.ToListAsync();
                _logger.LogInformation("Recuperados {ProductCount} produtos", products.Count);
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
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Produto não encontrado. ProductId = {ProductId}", id);
                }
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar produto. ProductId = {ProductId}", id);
                throw;
            }
        }
    }
}
