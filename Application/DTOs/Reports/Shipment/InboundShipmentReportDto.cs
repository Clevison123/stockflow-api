namespace StockFlow.Application.DTOs.Reports.Shipment
{
    public class InboundShipmentReportDto
    {
        public string ShipmentNumber { get; set; } = string.Empty;

        public string SupplierName { get; set; } = string.Empty;

        public DateTime ArrivalDate { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}