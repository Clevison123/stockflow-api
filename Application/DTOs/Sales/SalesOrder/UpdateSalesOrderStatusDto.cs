using StockFlow.Domain.Enums.Sales;

namespace StockFlow.Application.DTOs.Sales.SalesOrder
{
    public class UpdateSalesOrderStatusDto
    {
        public SalesOrderStatus Status { get; set; }
    }
}
