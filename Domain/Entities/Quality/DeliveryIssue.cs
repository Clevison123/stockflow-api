using StockFlow.Domain.Entities.Common;
using StockFlow.Domain.Entities.Logistics;
using StockFlow.Domain.Enums;

namespace StockFlow.Domain.Entities.Quality
{
    public class DeliveryIssue : BaseEntity
    {
        public int DeliveryId { get; set; }

        public Delivery Delivery { get; set; } = null!;

        public DeliveryIssueType IssueType { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool IsResolved { get; set; }

        public DateTime OccurredAt { get; set; }

        public DateTime? ResolvedAt { get; set; }

        public string ResolutionNotes { get; set; } = string.Empty;
    }
}
