using StockFlow.API.src.Application.Interfaces;
using StockFlow.API.src.Domain.Entities;
using StockFlow.API.src.Infrastructure.Data;

namespace StockFlow.API.src.Application.Services
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
