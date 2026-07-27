using StockFlow.Domain.Enums.Purchasing;

namespace StockFlow.Application.DTOs.SupplierClaim
{
    public class SupplierClaimResponseDto
    {
        public int Id { get; set; }

        public int SupplierId { get; set; }

        public string SupplierName { get; set; } = string.Empty;

        public SupplierClaimType ClaimType { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime OpenedAt { get; set; }

        public int OpenedByUserId { get; set; }

        public string OpenedByUserName { get; set; } = string.Empty;

        public bool IsResolved { get; set; }

        public DateTime? ResolvedAt { get; set; }

        public string ResolutionNotes { get; set; } = string.Empty;

        public int? QualityIssueId { get; set; }
    }
}