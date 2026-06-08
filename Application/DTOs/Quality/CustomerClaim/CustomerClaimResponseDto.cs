using StockFlow.Domain.Enums;

namespace StockFlow.Application.DTOs.Quality.CustomerClaim
{
    public class CustomerClaimResponseDto
    {
        public int Id { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string OrderNumber { get; set; } = string.Empty;

        public CustomerClaimType ClaimType { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime ReportedAt { get; set; }

        public bool IsResolved { get; set; }

        public DateTime? ResolvedAt { get; set; }

        public string ResolutionNotes { get; set; } = string.Empty;
    }
}
