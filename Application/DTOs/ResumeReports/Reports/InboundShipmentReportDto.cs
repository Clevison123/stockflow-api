namespace StockFlow.Application.DTOs.ResumeReports.Reports
{
    public class InboundShipmentReportDto
    {
        public string ShipmentNumber { get; set; } = string.Empty;

        public string SupplierName { get; set; } = string.Empty;

        public DateTime ArrivalDate { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}