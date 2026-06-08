namespace StockFlow.Application.DTOs.ResumeReports.Reports
{
    public class DeliveryReportDto
    {
        public int DeliveryId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public DateTime DeliveryDate { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}