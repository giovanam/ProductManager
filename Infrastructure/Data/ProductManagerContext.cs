using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Entities;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace ProductManager.Infrastructure.Data
{
    public class ProductManagerContext : DbContext
    {
        public ProductManagerContext(DbContextOptions<ProductManagerContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
