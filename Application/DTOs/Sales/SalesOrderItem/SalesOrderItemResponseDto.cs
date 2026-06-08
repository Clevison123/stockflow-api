namespace StockFlow.Application.DTOs.Sales.SalesOrderItem
{
    public class SalesOrderItemResponseDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
