namespace StockFlow.Application.DTOs.Catalog.Product
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Barcode { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public string OriginCountry { get; set; } = string.Empty;

        public int WarrantyMonths { get; set; }

        public bool HasSerialNumber { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SalePrice { get; set; }

        public int QuantityInStock { get; set; }

        public int MinimumStock { get; set; }

        public int CategoryId { get; set; }

        public int SupplierId { get; set; }
    }
}
