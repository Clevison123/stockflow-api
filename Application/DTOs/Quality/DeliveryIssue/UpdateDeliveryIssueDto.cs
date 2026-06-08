using StockFlow.Domain.Enums;

namespace StockFlow.Application.DTOs.Quality.DeliveryIssue
{
    public class UpdateDeliveryIssueDto
    {
        public DeliveryIssueType IssueType { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
