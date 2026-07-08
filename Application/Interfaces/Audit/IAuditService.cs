using StockFlow.Application.DTOs.Audit;

namespace StockFlow.Application.Interfaces.IAudit
{
    public interface IAuditService
    {
        Task CreateAuditLogAsync(CreateAuditLogDto dto);
    }
}