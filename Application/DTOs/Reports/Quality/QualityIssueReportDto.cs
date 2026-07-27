namespace StockFlow.Application.DTOs.Reports.Quality
{
    public class QualityIssueReportDto
    {
        public string ProductName { get; set; } = string.Empty;

        public string IssueType { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool RequiresSupplierClaim { get; set; }

        public bool IsResolved { get; set; }

        public DateTime DetectedAt { get; set; }
    }
}