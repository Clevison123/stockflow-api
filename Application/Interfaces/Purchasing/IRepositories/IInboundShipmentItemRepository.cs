using StockFlow.Domain.Entities.Purchasing;

namespace StockFlow.Application.Interfaces.Purchasing.IRepositories
{
    public interface IInboundShipmentItemRepository
    {
        Task AddRangeAsync(IEnumerable<InboundShipmentItem> items);

        Task DeleteByShipmentIdAsync(int shipmentId);

        Task<IEnumerable<InboundShipmentItem>> GetByShipmentIdAsync(int shipmentId);
    }
}
