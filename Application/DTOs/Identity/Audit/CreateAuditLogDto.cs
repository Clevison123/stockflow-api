namespace StockFlow.Application.DTOs.Identity.Audit
{
    public class CreateAuditLogDto
    {
        public string Action { get; set; } = string.Empty;

        public string EntityName { get; set; } = string.Empty;

        public string EntityId { get; set; } = string.Empty;

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }
    }
}
