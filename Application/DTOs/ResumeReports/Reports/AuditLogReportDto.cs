namespace StockFlow.Application.DTOs.ResumeReports.Reports
{
    public class AuditLogReportDto
    {
        public string UserEmail { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        public string EntityName { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }
    }
}