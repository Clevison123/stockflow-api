namespace StockFlow.Application.DTOs.Reports.Audit
{
    public class AuditLogReportDto
    {
        public DateTime Timestamp { get; set; }

        public string UserEmail { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        public string EntityName { get; set; } = string.Empty;

        public string EntityId { get; set; } = string.Empty;

        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }
    }
}