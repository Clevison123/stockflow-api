using StockFlow.Domain.Entities.Quality;

namespace StockFlow.Application.DTOs.Reports.Delivery
{
    public class DeliveryIssueReportDto
    {
        public int DeliveryId { get; set; }

        public string IssueType { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsResolved { get; set; }

        public DateTime OccurredAt { get; set; }

        public DateTime? ResolvedAt { get; set; }
    }
}