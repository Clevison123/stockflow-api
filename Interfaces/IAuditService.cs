using StockFlow.API.Entities;

namespace StockFlow.API.Interfaces
{
    public interface IAuditService
    {
        Task LogAsync(AuditLog log);
    }
}
