using StockFlow.API.src.Domain.Enums;

namespace StockFlow.API.src.Domain.Entities
{
    public class StockMovement : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public MovementType MovementType { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
