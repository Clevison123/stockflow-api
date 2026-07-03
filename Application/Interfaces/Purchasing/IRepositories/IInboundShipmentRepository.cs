using StockFlow.Domain.Entities.Purchasing;
using StockFlow.Domain.Enums;

namespace StockFlow.Application.Interfaces.Purchasing
{
    public interface IInboundShipmentRepository
    {
        // READ
        Task<IEnumerable<InboundShipment>> GetAllAsync();

        Task<InboundShipment?> GetByIdAsync(int id);

        Task<InboundShipment?> GetByShipmentNumberAsync(string number);

        Task<IEnumerable<InboundShipment>> GetBySupplierAsync(int supplierId);

        Task<IEnumerable<InboundShipment>> GetByStatusAsync(InboundShipmentStatus status);

        // INCLUDE ITEMS (IMPORTANTÍSSIMO)
        Task<InboundShipment?> GetWithItemsAsync(int id);

        // WRITE
        Task AddAsync(InboundShipment shipment);

        Task UpdateAsync(InboundShipment shipment);

        Task DeleteAsync(InboundShipment shipment);
    }
}