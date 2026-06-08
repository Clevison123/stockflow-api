using StockFlow.Domain.Entities.Common;
using StockFlow.Domain.Entities.Identity;
using StockFlow.Domain.Entities.Purchasing;
using StockFlow.Domain.Enums;

namespace StockFlow.Domain.Entities.Quality
{
    public class SupplierClaim : BaseEntity
    {
        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; } = null!;

        public SupplierClaimType ClaimType { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime OpenedAt { get; set; }

        public int OpenedByUserId { get; set; }

        public User OpenedByUser { get; set; } = null!;

        public bool IsResolved { get; set; }

        public DateTime? ResolvedAt { get; set; }

        public string ResolutionNotes { get; set; } = string.Empty;

        public int? QualityIssueId { get; set; }

        public QualityIssue? QualityIssue { get; set; }
    }
}