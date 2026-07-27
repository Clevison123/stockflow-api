using StockFlow.Domain.Enums.Sales;

namespace StockFlow.Application.DTOs.Quality.CustomerClaim
{
    public class CreateCustomerClaimDto
    {
        public int CustomerId { get; set; }

        public int SalesOrderId { get; set; }

        public CustomerClaimType ClaimType { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
