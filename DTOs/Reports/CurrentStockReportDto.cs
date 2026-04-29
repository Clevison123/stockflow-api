namespace StockFlow.API.DTOs.Reports
{
    public class CurrentStockReportDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public int QuantityInStock { get; set; }
        public int MinimumStock { get; set; }
        public decimal Price { get; set; }
        public decimal TotalStockValue { get; set; }
    }
}
