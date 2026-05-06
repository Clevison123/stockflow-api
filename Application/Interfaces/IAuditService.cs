using StockFlow.API.Domain.Entities;

namespace StockFlow.API.Application.Interfaces
{
    public interface IAuditService
    {
        Task LogAsync(AuditLog log);
    }
}
