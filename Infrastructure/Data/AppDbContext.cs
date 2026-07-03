using Microsoft.EntityFrameworkCore;
using StockFlow.Domain.Entities.Audit;
using StockFlow.Domain.Entities.Catalog;
using StockFlow.Domain.Entities.Common;
using StockFlow.Domain.Entities.Identity;
using StockFlow.Domain.Entities.Inventory;
using StockFlow.Domain.Entities.Logistics;
using StockFlow.Domain.Entities.Purchasing;
using StockFlow.Domain.Entities.Quality;
using StockFlow.Domain.Entities.Sales;

namespace StockFlow.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Identity
        public DbSet<User> Users => Set<User>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        // Catalog
        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Product> Products => Set<Product>();

        public DbSet<ProductItem> ProductItems => Set<ProductItem>();

        // Purchasing
        public DbSet<Supplier> Suppliers => Set<Supplier>();

        public DbSet<InboundShipment> InboundShipments => Set<InboundShipment>();

        public DbSet<InboundShipmentItem> InboundShipmentItems => Set<InboundShipmentItem>();

        // Inventory
        public DbSet<StockMovement> StockMovements => Set<StockMovement>();

        // Sales
        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();

        public DbSet<SalesOrderItem> SalesOrderItems => Set<SalesOrderItem>();

        // Logistics
        public DbSet<Delivery> Deliveries => Set<Delivery>();

        public DbSet<DeliveryIssue> DeliveryIssues => Set<DeliveryIssue>();

        // Quality
        public DbSet<CustomerClaim> CustomerClaims => Set<CustomerClaim>();

        public DbSet<SupplierClaim> SupplierClaims => Set<SupplierClaim>();

        public DbSet<QualityIssue> QualityIssues => Set<QualityIssue>();

        // Audit
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);
        }
    }
}