using StockFlow.Domain.Enums;

namespace StockFlow.Application.DTOs.SupplierClaim
{
    public class UpdateSupplierClaimDto
    {
        public SupplierClaimType ClaimType { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
