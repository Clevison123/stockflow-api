using StockFlow.Application.DTOs.Sales.SalesOrderItem;

namespace StockFlow.Application.DTOs.Sales.SalesOrder
{
    public class CreateSalesOrderDto
    {
        public int CustomerId { get; set; }

        public string Notes { get; set; } = string.Empty;

        public List<CreateSalesOrderItemDto> Items { get; set; }
            = new();
    }
}
