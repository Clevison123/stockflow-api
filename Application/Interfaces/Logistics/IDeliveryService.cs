using StockFlow.Application.DTOs.Logistics.Delivery;
using StockFlow.Domain.Enums.Logistics;

namespace StockFlow.Application.Interfaces.Logistics
{
    public interface IDeliveryService
    {
        Task<DeliveryResponseDto> CreateAsync(CreateDeliveryDto createDelivery);

        Task<DeliveryResponseDto> UpdateAsync(int id,UpdateDeliveryDto updateDelivery);

        Task<DeliveryResponseDto> GetByIdAsync(int id);

        Task<IEnumerable<DeliveryResponseDto>>GetAllAsync();

        Task<IEnumerable<DeliveryResponseDto>>GetByStatusAsync(DeliveryStatus deliveryStatus);

        Task<DeliveryResponseDto> UpdateStatusAsync(int id,UpdateDeliveryStatusDto dto);

        Task DeleteAsync(int id);
    }
}