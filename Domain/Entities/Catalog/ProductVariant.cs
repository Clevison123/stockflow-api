using StockFlow.Domain.Entities.Common;

namespace StockFlow.Domain.Entities.Catalog
{
    public class ProductVariant : BaseEntity
    {
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public string Name { get; set; } = string.Empty;

        public string? Color { get; set; }

        public string? Size { get; set; }

        public string? Storage { get; set; }

        public string? Memory { get; set; }

        public decimal SalePrice { get; set; }

        public bool IsActive { get; set; } = true;

        public List<ProductItem> ProductItems { get; set; } = new();
    }
}
