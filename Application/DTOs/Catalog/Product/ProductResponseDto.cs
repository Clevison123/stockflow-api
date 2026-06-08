namespace StockFlow.Application.DTOs.Catalog.Product
{
    public class ProductResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string SKU { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public string SupplierName { get; set; } = string.Empty;

        public string OriginCountry { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        public int WarrantyMonths { get; set; }

        public int MinimumStock { get; set; }

        public int CurrentStock { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
