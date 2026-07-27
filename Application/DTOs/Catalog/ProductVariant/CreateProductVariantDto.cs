namespace StockFlow.Application.DTOs.Catalog.ProductVariant
{
    public class CreateProductVariantDto
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Color { get; set; }

        public string? Size { get; set; }

        public string? Storage { get; set; }

        public string? Memory { get; set; }

        public decimal SalePrice { get; set; }
    }
}
