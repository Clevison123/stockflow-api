using StockFlow.Application.DTOs.Identity.Audit;

namespace StockFlow.Application.Interfaces.Identity
{
    public interface IAuditService
    {
        Task LogAsync(CreateAuditLogDto dto);
    }
}
