using Microsoft.EntityFrameworkCore;
using StockFlow.API.Application.Interfaces;
using StockFlow.API.Domain.Entities;
using StockFlow.API.Domain.Enums;
using StockFlow.API.DTOs.StockMovement;
using StockFlow.API.Application.Exceptions;
using StockFlow.API.Infrastructure.Data;
using System.Text.Json;

namespace StockFlow.API.Application.Services
{
    public class StockMovementService : IStockMovementService
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        private readonly IAuditService _auditService;

        public StockMovementService(AppDbContext context,
                                    ICurrentUserService currentUserService,
                                    IAuditService auditService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _auditService = auditService;
        }

        public async Task<StockMovement> RegisterEntryAsync(CreateStockMovementDto dto)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == dto.ProductId && !p.IsDeleted);

            if (product == null)
                throw new NotFoundException($"Product with ID {dto.ProductId} was not found.");

            var oldProduct = JsonSerializer.Serialize(product);

            product.QuantityInStock += dto.Quantity;
            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedByUserId = _currentUserService.UserId;

            var movement = new StockMovement
            {
                ProductId = dto.ProductId,
                MovementType = MovementType.In,
                Quantity = dto.Quantity,
                Reason = dto.Reason,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = _currentUserService.UserId
            };

            _context.StockMovements.Add(movement);
            await _context.SaveChangesAsync();

            await _auditService.LogAsync(new AuditLog
            {
                UserId = _currentUserService.UserId,
                UserEmail = _currentUserService.Email,
                Action = "STOCK_ENTRY",
                EntityName = "Product",
                EntityId = product.Id.ToString(),
                OldValues = oldProduct,
                NewValues = JsonSerializer.Serialize(product)
            });

            return movement;
        }

        public async Task<StockMovement> RegisterExitAsync(CreateStockMovementDto dto)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == dto.ProductId && !p.IsDeleted);

            if (product == null)
                throw new NotFoundException($"Product with ID {dto.ProductId} was not found.");

            if (product.QuantityInStock < dto.Quantity)
                throw new BusinessRuleException("Insufficient stock for this operation.");

            var oldProduct = JsonSerializer.Serialize(product);

            product.QuantityInStock -= dto.Quantity;
            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedByUserId = _currentUserService.UserId;

            var movement = new StockMovement
            {
                ProductId = dto.ProductId,
                MovementType = MovementType.Out,
                Quantity = dto.Quantity,
                Reason = dto.Reason,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = _currentUserService.UserId
            };

            _context.StockMovements.Add(movement);
            await _context.SaveChangesAsync();

            await _auditService.LogAsync(new AuditLog
            {
                UserId = _currentUserService.UserId,
                UserEmail = _currentUserService.Email,
                Action = "STOCK_EXIT",
                EntityName = "Product",
                EntityId = product.Id.ToString(),
                OldValues = oldProduct,
                NewValues = JsonSerializer.Serialize(product)
            });

            return movement;
        }

        public async Task<IEnumerable<StockMovement>> GetAllAsync()
        {
            return await _context.StockMovements
                .Where(sm => !sm.IsDeleted)
                .Include(sm => sm.Product)
                .OrderByDescending(sm => sm.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockMovement>> GetByProductIdAsync(int productId)
        {
            return await _context.StockMovements
                .Where(sm => sm.ProductId == productId && !sm.IsDeleted)
                .Include(sm => sm.Product)
                .OrderByDescending(sm => sm.CreatedAt)
                .ToListAsync();
        }
    }
}