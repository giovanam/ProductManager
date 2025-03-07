using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Entities;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace ProductManager.Infrastructure.Data
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
