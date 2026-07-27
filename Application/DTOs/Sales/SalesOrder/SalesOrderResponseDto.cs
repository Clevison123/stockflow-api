using StockFlow.Application.DTOs.Sales.SalesOrderItem;
using StockFlow.Domain.Enums.Sales;

namespace StockFlow.Application.DTOs.Sales.SalesOrder
{
    public class SalesOrderResponseDto
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }

        public SalesOrderStatus Status { get; set; }

        public decimal TotalAmount { get; set; }

        public string Notes { get; set; } = string.Empty;

        public List<SalesOrderItemResponseDto> Items { get; set; }
            = new();
    }
}
