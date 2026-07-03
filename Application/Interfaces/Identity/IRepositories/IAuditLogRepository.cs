using StockFlow.Domain.Entities.Audit;

namespace StockFlow.Application.Interfaces.Identity
{
    public interface IAuditLogRepository
    {
        Task AddAsync(AuditLog log);

        Task<IEnumerable<AuditLog>> GetAllAsync();

        Task<IEnumerable<AuditLog>> GetByUserIdAsync(int userId);

        Task<IEnumerable<AuditLog>> GetByEntityNameAsync(string entityName);
    }
}