namespace StockFlow.API.DTOs.Dashboard
{
    public class LowStockProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int QuantityInStock { get; set; }
        public int MinimumStock { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
    }
}
