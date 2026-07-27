using StockFlow.Domain.Enums.Quality;

namespace StockFlow.Application.DTOs.Quality.QualityIssue
{
    public class CreateQualityIssueDto
    {
        public int ProductItemId { get; set; }
        public QualityIssueType IssueType { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool RequiresSupplierClaim { get; set; }
    }
}