using StockFlow.Domain.Entities.Logistics;
using StockFlow.Domain.Enums.Logistics;

namespace StockFlow.Application.Interfaces.Logistics
{
    public interface IDeliveryRepository
    {
        // READ

        Task<IEnumerable<Delivery>> GetAllAsync();

        Task<Delivery?> GetByIdAsync(int id);

        Task<Delivery?> GetBySalesOrderIdAsync(int salesOrderId);

        Task<IEnumerable<Delivery>> GetByStatusAsync(DeliveryStatus deliveryStatus);

        Task<IEnumerable<Delivery>> GetPendingDeliveriesAsync();


        // WRITE

        Task AddAsync(Delivery addDelivery);

        Task UpdateAsync(Delivery updateDelivery);

        Task DeleteAsync(Delivery deleteDelivery);
    }
}