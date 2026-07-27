using StockFlow.Application.DTOs.InboundShipmentItem;

namespace StockFlow.Application.DTOs.Purchasing.InboundShipment
{
    public class CreateInboundShipmentDto
    {
        public string ShipmentNumber { get; set; } = string.Empty;

        public string ContainerNumber { get; set; } = string.Empty;

        public string OriginCountry { get; set; } = string.Empty;

        public DateTime ArrivalDate { get; set; }

        public int SupplierId { get; set; }

        public string Notes { get; set; } = string.Empty;

        public List<CreateInboundShipmentItemDto>
            Items
        { get; set; } = [];
    }
}
