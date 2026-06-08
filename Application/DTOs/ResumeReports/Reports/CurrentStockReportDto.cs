namespace StockFlow.Application.DTOs.ResumeReports.Reports
{
    public class CurrentStockReportDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public int QuantityInStock { get; set; }

        public int MinimumStock { get; set; }
    }
}