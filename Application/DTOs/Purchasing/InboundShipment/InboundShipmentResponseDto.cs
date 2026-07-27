using StockFlow.Application.DTOs.Purchasing.InboundShipmentItem;
using StockFlow.Domain.Enums;

namespace StockFlow.Application.DTOs.Purchasing.InboundShipment
{
    public class InboundShipmentResponseDto
    {
        public int Id { get; set; }

        public string ShipmentNumber { get; set; } = string.Empty;

        public string ContainerNumber { get; set; } = string.Empty;

        public string OriginCountry { get; set; } = string.Empty;

        public DateTime ArrivalDate { get; set; }

        public InboundShipmentStatus Status { get; set; }

        public string SupplierName { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
        public IEnumerable<InboundShipmentItemResponseDto> Items { get; set; } = new List<InboundShipmentItemResponseDto>();
    }
}
