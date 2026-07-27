using StockFlow.Domain.Enums;

namespace StockFlow.Application.DTOs.Purchasing.InboundShipment
{
    public class UpdateInboundShipmentStatusDto
    {
        public InboundShipmentStatus Status { get; set; }
    }
}