using StockFlow.Domain.Entities.Catalog;
using StockFlow.Domain.Entities.Common;
using StockFlow.Domain.Enums;

namespace StockFlow.Domain.Entities.Inventory
{
    public class StockMovement : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public MovementType MovementType { get; set; }

        public int Quantity { get; set; }

        public int PreviousQuantity { get; set; }

        public int CurrentQuantity { get; set; }

        public string Reason { get; set; } = string.Empty;
    }
}