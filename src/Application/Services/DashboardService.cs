using Microsoft.EntityFrameworkCore;
using StockFlow.API.Domain.Entities;
using StockFlow.API.src.Application.Interfaces;
using StockFlow.API.src.Application.DTOs.Dashboard;
using StockFlow.API.src.Domain.Enums;
using StockFlow.API.src.Infrastructure.Data;

namespace StockFlow.API.src.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LowStockProductDto>> GetLowStockProductsAsync()
        {
            return await _context.Products
                .Where(p => !p.IsDeleted && p.QuantityInStock <= p.MinimumStock)
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .OrderBy(p => p.QuantityInStock)
                .Select(p => new LowStockProductDto
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    QuantityInStock = p.QuantityInStock,
                    MinimumStock = p.MinimumStock,
                    CategoryName = p.Category.Name,
                    SupplierName = p.Supplier.Name
                })
                .ToListAsync();
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);

            var totalProducts = await _context.Products.CountAsync(p => !p.IsDeleted);
            var totalCategories = await _context.Categories.CountAsync(c => !c.IsDeleted);
            var totalSuppliers = await _context.Suppliers.CountAsync(s => !s.IsDeleted);

            var lowStockProducts = await _context.Products
                .CountAsync(p => !p.IsDeleted && p.QuantityInStock <= p.MinimumStock);

            var outOfStockProducts = await _context.Products
                .CountAsync(p => !p.IsDeleted && p.QuantityInStock == 0);

            var totalStockMovements = await _context.StockMovements
                .CountAsync(sm => !sm.IsDeleted);

            var recentEntries = await _context.StockMovements
                .CountAsync(sm => !sm.IsDeleted &&
                                  sm.MovementType == MovementType.In &&
                                  sm.CreatedAt >= sevenDaysAgo);

            var recentExits = await _context.StockMovements
                .CountAsync(sm => !sm.IsDeleted &&
                                  sm.MovementType == MovementType.Out &&
                                  sm.CreatedAt >= sevenDaysAgo);

            return new DashboardSummaryDto
            {
                TotalProducts = totalProducts,
                TotalCategories = totalCategories,
                TotalSuppliers = totalSuppliers,
                LowStockProducts = lowStockProducts,
                OutOfStockProducts = outOfStockProducts,
                TotalStockMovements = totalStockMovements,
                RecentEntries = recentEntries,
                RecentExits = recentExits
            };
        }

        public async Task<IEnumerable<RecentMovementDto>> GetRecentMovementsAsync(int count = 10)
        {
            return await _context.StockMovements
                .Where(sm => !sm.IsDeleted)
                .Include(sm => sm.Product)
                .OrderByDescending(sm => sm.CreatedAt)
                .Take(count)
                .Select(sm => new RecentMovementDto
                {
                    MovementId = sm.Id,
                    ProductName = sm.Product.Name,
                    MovementType = sm.MovementType.ToString(),
                    Quantity = sm.Quantity,
                    Reason = sm.Reason,
                    CreatedAt = sm.CreatedAt
                })
                .ToListAsync();
        }
    }
}