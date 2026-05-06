namespace StockFlow.API.DTOs.Reports
{
    public class LowStockReportDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public int QuantityInStock { get; set; }
        public int MinimumStock { get; set; }
        public int MissingQuantity { get; set; }
    }
}
