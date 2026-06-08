namespace StockFlow.Application.DTOs.Catalog.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string SKU { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public int SupplierId { get; set; }

        public string OriginCountry { get; set; } = string.Empty;

        public int WarrantyMonths { get; set; }

        public decimal UnitPrice { get; set; }

        public int MinimumStock { get; set; }
    }
}
