using StockFlow.Domain.Enums;

namespace StockFlow.Application.DTOs.SupplierClaim
{
    public class CreateSupplierClaimDto
    {
        public int SupplierId { get; set; }

        public SupplierClaimType ClaimType { get; set; }

        public string Description { get; set; } = string.Empty;

        public int? QualityIssueId { get; set; }
    }
}
