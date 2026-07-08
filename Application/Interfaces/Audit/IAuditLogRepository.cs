using StockFlow.Domain.Entities.Audit;
using StockFlow.Domain.Enums;

namespace StockFlow.Application.Interfaces.IAudit
{
    public interface IAuditLogRepository
    {
        Task AddAsync(AuditLog log);

        Task<IEnumerable<AuditLog>> GetAllAsync();

        Task<IEnumerable<AuditLog>> GetByUserIdAsync(int userId);

        Task<IEnumerable<AuditLog>> GetByEntityAsync(AuditEntity entity);
    }
}