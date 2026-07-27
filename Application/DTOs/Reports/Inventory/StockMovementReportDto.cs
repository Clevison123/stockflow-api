using StockFlow.Domain.Enums.Inventory;

namespace StockFlow.Application.DTOs.Reports
{
    public class StockMovementReportDto
    {
        public string ProductName { get; set; } = string.Empty;

        public MovementType MovementType { get; set; }

        public int Quantity { get; set; }

        public string Reason { get; set; } = string.Empty;

        public DateTime Date { get; set; }
    }
}