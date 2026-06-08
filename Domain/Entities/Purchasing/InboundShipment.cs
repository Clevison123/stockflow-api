using StockFlow.Domain.Entities.Common;
using StockFlow.Domain.Entities.Purchasing;
using StockFlow.Domain.Enums;

public class InboundShipment : BaseEntity
{
    public string ShipmentNumber { get; set; } = string.Empty;

    public string ContainerNumber { get; set; } = string.Empty;

    public string OriginCountry { get; set; } = string.Empty;

    public DateTime ArrivalDate { get; set; }

    public string Notes { get; set; } = string.Empty;

    public InboundShipmentStatus Status { get; set; }

    public int SupplierId { get; set; }

    public Supplier Supplier { get; set; } = null!;
}