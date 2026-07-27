namespace StockFlow.Application.DTOs.Reports.Sales
{
    public class CustomerSalesReportDto
    {
        public string CustomerName { get; set; } = string.Empty;

        public int TotalOrders { get; set; }

        public decimal TotalPurchased { get; set; }

        public bool IsPriorityCustomer { get; set; }
    }
}