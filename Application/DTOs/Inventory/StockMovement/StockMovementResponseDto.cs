using StockFlow.Domain.Enums.Inventory;

namespace StockFlow.Application.DTOs.Inventory.StockMovement
{
    public class StockMovementResponseDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public MovementType MovementType { get; set; }

        public int Quantity { get; set; }

        public int PreviousQuantity { get; set; }

        public int CurrentQuantity { get; set; }

        public string Reason { get; set; } = string.Empty;

        public int? PerformedByUserId { get; set; }

        public string? PerformedByUserName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}