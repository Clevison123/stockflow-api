using StockFlow.Application.DTOs.Purchasing.InboundShipment;
using StockFlow.Domain.Enums;

namespace StockFlow.Application.Interfaces.Purchasing
{
    public interface IInboundShipmentService
    {
        Task<InboundShipmentResponseDto>
            CreateAsync(
                CreateInboundShipmentDto dto);

        Task<InboundShipmentResponseDto>
            UpdateAsync(
                int id,
                UpdateInboundShipmentDto dto);

        Task<InboundShipmentResponseDto>
            GetByIdAsync(
                int id);

        Task<IEnumerable<InboundShipmentResponseDto>>
            GetAllAsync();

        Task<IEnumerable<InboundShipmentResponseDto>>
            GetBySupplierAsync(
                int supplierId);

        Task<IEnumerable<InboundShipmentResponseDto>>
            GetByStatusAsync(
                InboundShipmentStatus status);

        Task UpdateStatusAsync(
            int shipmentId,
            InboundShipmentStatus status);

        Task ConfirmReceivedAsync(
            int shipmentId);

        Task CancelAsync(
            int shipmentId,
            string reason);
    }
}
