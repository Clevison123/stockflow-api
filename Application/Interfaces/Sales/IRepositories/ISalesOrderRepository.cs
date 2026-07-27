using StockFlow.Domain.Entities.Sales;
using StockFlow.Domain.Enums.Sales;

namespace StockFlow.Application.Interfaces.Sales
{
    public interface ISalesOrderRepository
    {
        // READ
        Task<IEnumerable<SalesOrder>> GetAllAsync();

        Task<SalesOrder?> GetByIdAsync(int id);

        Task<SalesOrder?> GetByOrderNumberAsync(string orderNumber);

        Task<IEnumerable<SalesOrder>> GetByCustomerIdAsync(int customerId);

        Task<IEnumerable<SalesOrder>> GetByStatusAsync(SalesOrderStatus status);

        // IMPORTANTÍSSIMO: incluir itens
        Task<SalesOrder?> GetWithItemsAsync(int id);

        // WRITE
        Task AddAsync(SalesOrder order);

        Task UpdateAsync(SalesOrder order);

        Task DeleteAsync(SalesOrder order);
    }
}