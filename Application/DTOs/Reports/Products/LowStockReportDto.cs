namespace StockFlow.Application.DTOs.Reports.Catalog
{
    public class LowStockReportDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int CurrentStock { get; set; }

        public int MinimumStock { get; set; }
    }
}