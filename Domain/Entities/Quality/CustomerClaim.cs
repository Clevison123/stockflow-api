using StockFlow.Domain.Entities.Common;
using StockFlow.Domain.Entities.Sales;
using StockFlow.Domain.Enums;

namespace StockFlow.Domain.Entities.Quality
{
    public class CustomerClaim : BaseEntity
    {
        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public int SalesOrderId { get; set; }

        public SalesOrder SalesOrder { get; set; } = null!;

        public CustomerClaimType ClaimType { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime ReportedAt { get; set; }

        public bool IsResolved { get; set; }

        public DateTime? ResolvedAt { get; set; }

        public string ResolutionNotes { get; set; } = string.Empty;
    }
}