using ProductManager.Domain.Entities;

namespace ProductManager.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
    }
}
