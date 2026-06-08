using StockFlow.Domain.Enums;

namespace StockFlow.Application.DTOs.Quality.DeliveryIssue
{
    public class CreateDeliveryIssueDto
    {
        public int DeliveryId { get; set; }

        public DeliveryIssueType IssueType { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
