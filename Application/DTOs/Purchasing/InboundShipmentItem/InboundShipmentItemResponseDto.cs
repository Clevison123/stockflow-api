namespace StockFlow.Application.DTOs.Purchasing.InboundShipmentItem
{
    public class InboundShipmentItemResponseDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }
    }
}