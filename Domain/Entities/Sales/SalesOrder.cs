using StockFlow.Domain.Entities.Common;
using StockFlow.Domain.Enums;

namespace StockFlow.Domain.Entities.Sales
{
    public class SalesOrder : BaseEntity
    {
        public string OrderNumber { get; set; } = string.Empty;

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public SalesOrderStatus Status { get; set; }

        public decimal TotalAmount { get; set; }

        public string Notes { get; set; } = string.Empty;

        public ICollection<SalesOrderItem> Items { get; set; }
            = new List<SalesOrderItem>();
    }
}