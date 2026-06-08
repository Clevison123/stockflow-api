using StockFlow.Application.Interfaces.Identity;
using StockFlow.Domain.Entities.Identity;
using StockFlow.Infrastructure.Data;

namespace StockFlow.Application.Services
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
