using StockFlow.Domain.Entities.Common;
using StockFlow.Domain.Enums;

namespace StockFlow.Domain.Entities.Catalog
{
    public class ProductItem : BaseEntity
    {
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public string SerialNumber { get; set; } = string.Empty;

        public ProductItemStatus Status { get; set; }

        public DateTime ReceivedAt { get; set; }

        public DateTime? WarrantyUntil { get; set; }
    }
}