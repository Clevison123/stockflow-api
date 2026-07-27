namespace StockFlow.Application.DTOs.Reports.Inventory
{
    public class InventoryValueReportDto
    {
        public string ProductName { get; set; }

        public int QuantityInStock { get; set; }

        public decimal UnitCost { get; set; }

        public decimal TotalValue { get; set; }
    }
}
