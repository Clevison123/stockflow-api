namespace StockFlow.Application.DTOs.Reports.Sales
{
    public class SalesOrderReportDto
    {
        public string OrderNumber { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }
    }
}