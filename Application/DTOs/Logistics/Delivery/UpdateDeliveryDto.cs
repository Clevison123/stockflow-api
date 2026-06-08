namespace StockFlow.Application.DTOs.Logistics.Delivery
{
    public class UpdateDeliveryDto
    {
        public string DriverName { get; set; } = string.Empty;

        public string VehiclePlate { get; set; } = string.Empty;

        public string DeliveryAddress { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
    }
}
