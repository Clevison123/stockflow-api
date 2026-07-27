namespace StockFlow.Application.DTOs.Catalog.Product
{
    public class CurrentStockProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string SKU { get; set; } = string.Empty;

        public int QuantityInStock { get; set; }

        public int MinimumStock { get; set; }

        public bool IsLowStock { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public string SupplierName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime LastUpdatedAt { get; set; }
    }
}
