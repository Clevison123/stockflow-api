using StockFlow.API.src.Domain.Entities;

namespace StockFlow.API.src.Application.Interfaces
{
    public interface IAuditService
    {
        Task LogAsync(AuditLog log);
    }
}
