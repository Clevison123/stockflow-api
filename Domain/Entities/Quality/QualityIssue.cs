using StockFlow.Domain.Entities.Catalog;
using StockFlow.Domain.Entities.Common;
using StockFlow.Domain.Entities.Identity;
using StockFlow.Domain.Enums;

namespace StockFlow.Domain.Entities.Quality
{
    public class QualityIssue : BaseEntity
    {
        public int ProductItemId { get; set; }

        public ProductItem ProductItem { get; set; } = null!;

        public QualityIssueType IssueType { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime DetectedAt { get; set; }

        public int DetectedByUserId { get; set; }

        public User DetectedByUser { get; set; } = null!;

        public bool RequiresSupplierClaim { get; set; }

        public bool IsResolved { get; set; }

        public DateTime? ResolvedAt { get; set; }

        public string ResolutionNotes { get; set; } = string.Empty;
    }
}