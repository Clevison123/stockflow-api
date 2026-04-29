namespace StockFlow.API.DTOs.Reports
{
    public class StockMovementReportDto
    {
        public int MovementId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string MovementType { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int? CreatedByUserId { get; set; }
    }
}
