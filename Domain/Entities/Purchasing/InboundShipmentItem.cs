using StockFlow.Domain.Entities.Catalog;
using StockFlow.Domain.Entities.Common;

namespace StockFlow.Domain.Entities.Purchasing
{
    public class InboundShipmentItem : BaseEntity
    {
        public int InboundShipmentId { get; set; }

        public InboundShipment InboundShipment { get; set; } = null!;

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
