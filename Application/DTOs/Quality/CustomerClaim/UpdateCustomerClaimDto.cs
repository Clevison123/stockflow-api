using StockFlow.Domain.Enums;

namespace StockFlow.Application.DTOs.Quality.CustomerClaim
{
    public class UpdateCustomerClaimDto
    {
        public CustomerClaimType ClaimType { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
