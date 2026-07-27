using StockFlow.Domain.Entities.Common;
using StockFlow.Domain.Entities.Sales;
using StockFlow.Domain.Entities.Quality;
using StockFlow.Domain.Enums.Logistics;

namespace StockFlow.Domain.Entities.Logistics
{
    public class Delivery : BaseEntity
    {
        public int SalesOrderId { get; set; }

        public SalesOrder SalesOrder { get; set; } = null!;

        public DeliveryStatus Status { get; set; }

        public DeliveryIssue deliveryIssue { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime? DeliveredAt { get; set; }

        public string DeliveryAddress { get; set; } = string.Empty;

        public string DriverName { get; set; } = string.Empty;

        public string VehiclePlate { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
    }
}
