namespace StockFlow.API.src.Application.DTOs.Audit
{
    public class AuditLogDto
    {
        public string UserEmail { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
