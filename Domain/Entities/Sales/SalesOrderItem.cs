using StockFlow.Domain.Entities.Catalog;
using StockFlow.Domain.Entities.Common;

namespace StockFlow.Domain.Entities.Sales
{
    public class SalesOrderItem : BaseEntity
    {
        public int SalesOrderId { get; set; }

        public SalesOrder SalesOrder { get; set; } = null!;

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }
}