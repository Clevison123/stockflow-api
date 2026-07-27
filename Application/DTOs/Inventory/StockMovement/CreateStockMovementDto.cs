using StockFlow.Domain.Enums.Inventory;

namespace StockFlow.Application.DTOs.Inventory.StockMovement
{
    public class CreateStockMovementDto
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public MovementType MovementType { get; set; }

        public string Reason { get; set; } = string.Empty;
    }
}
