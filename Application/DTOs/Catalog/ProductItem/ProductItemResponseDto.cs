using StockFlow.Application.DTOs.Catalog.ProductVariant;
using StockFlow.Domain.Enums.Catalog;

namespace StockFlow.Application.DTOs.Catalog.ProductItem
{
    public class ProductItemResponseDto
    {
        public int Id { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public ProductVariantSummaryDto Variant { get; set; } = null!;

        public string SerialNumber { get; set; } = string.Empty;

        public ProductItemStatus Status { get; set; }

        public DateTime ReceivedAt { get; set; }

        public DateTime? WarrantyUntil { get; set; }
    }
}
