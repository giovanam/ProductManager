using ProductManager.Application.DTOs;
using ProductManager.Domain.Entities;

namespace ProductManager.Domain.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>>GetAllAsync();
        Task AddProductAsync(ProductDTO productDTO);
        Task UpdateProductAsync(ProductDTO productDTO);
        Task DeleteProductAsync(int id);
        Task<Product> GetByIdAsync(int id);
    }
}
