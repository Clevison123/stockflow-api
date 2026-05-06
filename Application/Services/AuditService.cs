using StockFlow.API.Application.Interfaces;
using StockFlow.API.Domain.Entities;
using StockFlow.API.Infrastructure.Data;

namespace StockFlow.API.Application.Services
{
    public class AuditService : IAuditService
    {
        private readonly AppDbContext _context;

        public AuditService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(AuditLog log)
        {
            await _context.AuditLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
