using StockFlow.Application.DTOs.Purchasing.Supplier;
using StockFlow.Application.Interfaces.IAudit;
using StockFlow.Application.Interfaces.Identity;
using StockFlow.Domain.Entities.Identity;
using StockFlow.Domain.Entities.Purchasing;
using StockFlow.Infrastructure.Data;
using System.Text.Json;

namespace StockFlow.Application.Services
{
    public class SupplierService
    {
        private readonly AppDbContext _context;

        private readonly IAuditService _auditService;
        private readonly ICurrentUserService _currentUserService;

        public SupplierService(AppDbContext context,
                               IAuditService auditService,
                               ICurrentUserService currentUserService)
        {
            _context = context;
            _auditService = auditService;
            _currentUserService = currentUserService;
        }

        public async Task<Supplier> CreateSupplierAsync(CreateSupplierDto dto)
        {
            var supplier = new Supplier
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            await _auditService.LogAsync(new AuditLog
            {
                UserId = _currentUserService.UserId,
                UserEmail = _currentUserService.Email,
                Action = "CREATE",
                EntityName = "Supplier",
                EntityId = supplier.Id.ToString(),
                NewValues = JsonSerializer.Serialize(supplier)
            });

            return supplier;
        }

        public List<Supplier> GetAllSuppliers()
        {
            return _context.Suppliers.ToList();
        }

        public Supplier? GetSupplierById(int id)
        {
            return _context.Suppliers.FirstOrDefault(s => s.Id == id);
        }

        public async Task<Supplier?> UpdateSupplierAsync(int id, UpdateSupplierDto dto)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == id);

            if (supplier == null)
                return null;

            var oldValues = JsonSerializer.Serialize(supplier);

            supplier.Name = dto.Name;
            supplier.Email = dto.Email;
            supplier.Phone = dto.Phone;

            await _context.SaveChangesAsync();

            await _auditService.LogAsync(new AuditLog
            {
                UserId = _currentUserService.UserId,
                UserEmail = _currentUserService.Email,
                Action = "UPDATE",
                EntityName = "Supplier",
                EntityId = supplier.Id.ToString(),
                OldValues = oldValues,
                NewValues = JsonSerializer.Serialize(supplier)
            });

            return supplier;
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == id);

            if (supplier == null)
                return false;

            var oldValues = JsonSerializer.Serialize(supplier);

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            await _auditService.LogAsync(new AuditLog
            {
                UserId = _currentUserService.UserId,
                UserEmail = _currentUserService.Email,
                Action = "DELETE",
                EntityName = "Supplier",
                EntityId = supplier.Id.ToString(),
                OldValues = oldValues
            });

            return true;
        }
    }
}