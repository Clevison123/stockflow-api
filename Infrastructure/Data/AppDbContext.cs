using Microsoft.EntityFrameworkCore;
using StockFlow.Domain.Entities.Catalog;
using StockFlow.Domain.Entities.Identity;
using StockFlow.Domain.Entities.Inventory;
using StockFlow.Domain.Entities.Purchasing;

namespace StockFlow.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
    }
}
