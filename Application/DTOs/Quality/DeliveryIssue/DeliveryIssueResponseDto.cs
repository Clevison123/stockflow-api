using StockFlow.Domain.Enums;

namespace StockFlow.Application.DTOs.Quality.DeliveryIssue
{
    public class DeliveryIssueResponseDto
    {
        public int Id { get; set; }

        public int DeliveryId { get; set; }

        public DeliveryIssueType IssueType { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool IsResolved { get; set; }

        public DateTime OccurredAt { get; set; }

        public DateTime? ResolvedAt { get; set; }

        public string ResolutionNotes { get; set; } = string.Empty;
    }
}
