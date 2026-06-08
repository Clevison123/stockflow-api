namespace StockFlow.Application.DTOs.Purchasing.InboundShipment
{
    public class UpdateInboundShipmentDto
    {
        public DateTime ArrivalDate { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}
