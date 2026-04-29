using Microsoft.EntityFrameworkCore;
using StockFlow.API.Data;
using StockFlow.API.DTOs.Reports;
using StockFlow.API.Entities;
using StockFlow.API.Enums;
using StockFlow.API.Helpers;
using StockFlow.API.Helpers.Export;
using StockFlow.API.Interfaces;
using System.Text.Json; 

namespace StockFlow.API.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;

        private readonly IAuditService _auditService;
        private readonly ICurrentUserService _currentUserService;

        public ReportService(AppDbContext context,
                             IAuditService auditService,
                             ICurrentUserService currentUserService)
        {
            _context = context;
            _auditService = auditService;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<CurrentStockReportDto>> GetCurrentStockReportAsync()
        {
            return await _context.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .OrderBy(p => p.Name)
                .Select(p => new CurrentStockReportDto
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    CategoryName = p.Category.Name,
                    SupplierName = p.Supplier.Name,
                    QuantityInStock = p.QuantityInStock,
                    MinimumStock = p.MinimumStock,
                    Price = p.Price,
                    TotalStockValue = p.Price * p.QuantityInStock
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<LowStockReportDto>> GetLowStockReportAsync()
        {
            return await _context.Products
                .Where(p => !p.IsDeleted && p.QuantityInStock <= p.MinimumStock)
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .OrderBy(p => p.QuantityInStock)
                .Select(p => new LowStockReportDto
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    CategoryName = p.Category.Name,
                    SupplierName = p.Supplier.Name,
                    QuantityInStock = p.QuantityInStock,
                    MinimumStock = p.MinimumStock,
                    MissingQuantity = p.MinimumStock - p.QuantityInStock
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<StockMovementReportDto>> GetStockMovementReportAsync(StockMovementReportQueryParameters queryParameters)
        {
            var query = _context.StockMovements
                .Where(sm => !sm.IsDeleted)
                .Include(sm => sm.Product)
                .AsQueryable();

            if (queryParameters.ProductId.HasValue)
            {
                query = query.Where(sm => sm.ProductId == queryParameters.ProductId.Value);
            }

            if (!string.IsNullOrWhiteSpace(queryParameters.MovementType) &&
                Enum.TryParse<MovementType>(queryParameters.MovementType, true, out var movementType))
            {
                query = query.Where(sm => sm.MovementType == movementType);
            }

            if (queryParameters.StartDate.HasValue)
            {
                query = query.Where(sm => sm.CreatedAt >= queryParameters.StartDate.Value);
            }

            if (queryParameters.EndDate.HasValue)
            {
                query = query.Where(sm => sm.CreatedAt <= queryParameters.EndDate.Value);
            }

            return await query
                .OrderByDescending(sm => sm.CreatedAt)
                .Select(sm => new StockMovementReportDto
                {
                    MovementId = sm.Id,
                    ProductId = sm.ProductId,
                    ProductName = sm.Product.Name,
                    MovementType = sm.MovementType.ToString(),
                    Quantity = sm.Quantity,
                    Reason = sm.Reason,
                    CreatedAt = sm.CreatedAt,
                    CreatedByUserId = sm.CreatedByUserId
                })
                .ToListAsync();
        }

        public async Task<byte[]> ExportCurrentStockCsvAsync()
        {
            var data = await GetCurrentStockReportAsync();

            await _auditService.LogAsync(new AuditLog
            {
                UserId = _currentUserService.UserId,
                UserEmail = _currentUserService.Email,
                Action = "EXPORT",
                EntityName = "CurrentStockReport",
                NewValues = JsonSerializer.Serialize(new { Format = "CSV" })
            });

            return CsvExportHelper.ExportToCsv(data);
        }

        public async Task<byte[]> ExportCurrentStockExcelAsync()
        {
            var data = await GetCurrentStockReportAsync();

            await _auditService.LogAsync(new AuditLog
            {
                UserId = _currentUserService.UserId,
                UserEmail = _currentUserService.Email,
                Action = "EXPORT",
                EntityName = "CurrentStockReport",
                NewValues = JsonSerializer.Serialize(new { Format = "Excel" })
            });

            return ExcelExportHelper.ExportToExcel(data, "CurrentStock");
        }

        public async Task<byte[]> ExportStockMovementsCsvAsync(StockMovementReportQueryParameters query)
        {
            var data = await GetStockMovementReportAsync(query);

            await _auditService.LogAsync(new AuditLog
            {
                UserId = _currentUserService.UserId,
                UserEmail = _currentUserService.Email,
                Action = "EXPORT",
                EntityName = "StockMovementsReport",
                NewValues = JsonSerializer.Serialize(new { Format = "CSV" })
            });

            return CsvExportHelper.ExportToCsv(data);
        }

        public async Task<byte[]> ExportStockMovementsExcelAsync(StockMovementReportQueryParameters query)
        {
            var data = await GetStockMovementReportAsync(query);

            await _auditService.LogAsync(new AuditLog
            {
                UserId = _currentUserService.UserId,
                UserEmail = _currentUserService.Email,
                Action = "EXPORT",
                EntityName = "StockMovementsReport",
                NewValues = JsonSerializer.Serialize(new { Format = "Excel" })
            });

            return ExcelExportHelper.ExportToExcel(data, "StockMovements");
        }
    }
}