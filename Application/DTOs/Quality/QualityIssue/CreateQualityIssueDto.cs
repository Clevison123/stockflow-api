using StockFlow.Domain.Enums;

namespace StockFlow.Application.DTOs.Quality.QualityIssue
{
    public class CreateQualityIssueDto
    {
        public int ProductItemId { get; set; }

        public QualityIssueType IssueType { get; set; }

        public string Description { get; set; } = string.Empty;

        public int DetectedByUserId { get; set; }

        public bool RequiresSupplierClaim { get; set; }
    }
}
