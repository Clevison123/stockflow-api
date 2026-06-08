using StockFlow.Application.DTOs.Logistics.Delivery;
using StockFlow.Domain.Enums;

namespace StockFlow.Application.Interfaces.Logistics
{
    public interface IDeliveryService
    {
        Task<DeliveryResponseDto> CreateAsync(
            CreateDeliveryDto dto);

        Task<DeliveryResponseDto> UpdateAsync(
            int id,
            UpdateDeliveryDto dto);

        Task<DeliveryResponseDto> GetByIdAsync(
            int id);

        Task<IEnumerable<DeliveryResponseDto>>
            GetAllAsync();

        Task<IEnumerable<DeliveryResponseDto>>
            GetByStatusAsync(
                DeliveryStatus status);

        Task StartDeliveryAsync(
            int deliveryId);

        Task CompleteDeliveryAsync(
            int deliveryId);

        Task CancelDeliveryAsync(
            int deliveryId,
            string reason);
    }
}
