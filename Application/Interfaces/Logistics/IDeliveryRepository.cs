using StockFlow.Domain.Entities.Logistics;
using StockFlow.Domain.Enums;

namespace StockFlow.Application.Interfaces.Logistics
{
    public interface IDeliveryRepository
    {
        // READ
        Task<IEnumerable<Delivery>> GetAllAsync();

        Task<Delivery?> GetByIdAsync(int id);

        Task<Delivery?> GetBySalesOrderIdAsync(int salesOrderId);

        Task<IEnumerable<Delivery>> GetByStatusAsync(DeliveryStatus status);

        Task<IEnumerable<Delivery>> GetPendingDeliveriesAsync();

        // WRITE
        Task AddAsync(Delivery delivery);

        Task UpdateAsync(Delivery delivery);

        Task DeleteAsync(Delivery delivery);
    }
}