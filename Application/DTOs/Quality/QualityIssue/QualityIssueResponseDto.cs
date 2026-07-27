using StockFlow.Domain.Enums.Quality;
public class QualityIssueResponseDto
{
    public int Id { get; set; }

    public int ProductItemId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public QualityIssueType IssueType { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime DetectedAt { get; set; }

    public int DetectedByUserId { get; set; }

    public string DetectedByUserName { get; set; } = string.Empty;

    public bool RequiresSupplierClaim { get; set; }

    public bool IsResolved { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public string ResolutionNotes { get; set; } = string.Empty;
}