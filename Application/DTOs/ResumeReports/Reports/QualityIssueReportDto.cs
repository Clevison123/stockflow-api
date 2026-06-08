namespace StockFlow.Application.DTOs.ResumeReports.Reports
{
    public class QualityIssueReportDto
    {
        public string ProductName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}