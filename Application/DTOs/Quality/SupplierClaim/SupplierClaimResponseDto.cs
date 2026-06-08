using StockFlow.Domain.Enums;

namespace StockFlow.Application.DTOs.SupplierClaim
{
    public class SupplierClaimResponseDto
    {
        public int Id { get; set; }

        public string SupplierName { get; set; } = string.Empty;

        public SupplierClaimType ClaimType { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime OpenedAt { get; set; }

        public string OpenedBy { get; set; } = string.Empty;

        public bool IsResolved { get; set; }

        public DateTime? ResolvedAt { get; set; }

        public string ResolutionNotes { get; set; } = string.Empty;

        public int? QualityIssueId { get; set; }
    }
}
