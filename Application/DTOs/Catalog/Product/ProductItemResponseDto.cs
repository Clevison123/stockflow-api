using StockFlow.Domain.Enums;

namespace StockFlow.Application.DTOs.Catalog.Product
{
    public class ProductItemResponseDto
    {
        public int Id { get; set; }

        public string SerialNumber { get; set; } = string.Empty;

        public bool IsSold { get; set; }

        public bool IsDefective { get; set; }

        public ProductItemStatus Status { get; set; }

        public DateTime? WarrantyUntil { get; set; }
    }
}
