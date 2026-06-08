using StockFlow.Domain.Enums;

namespace StockFlow.Application.DTOs.Logistics.Delivery
{
    public class DeliveryResponseDto
    {
        public int Id { get; set; }

        public int SalesOrderId { get; set; }

        public DeliveryStatus Status { get; set; }

        public string DriverName { get; set; } = string.Empty;

        public string VehiclePlate { get; set; } = string.Empty;

        public string DeliveryAddress { get; set; } = string.Empty;

        public DateTime DepartureDate { get; set; }

        public DateTime? DeliveredAt { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}
