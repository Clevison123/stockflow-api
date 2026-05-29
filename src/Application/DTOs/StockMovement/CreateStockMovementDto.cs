namespace StockFlow.API.src.Application.DTOs.StockMovement
{
    public class CreateStockMovementDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
