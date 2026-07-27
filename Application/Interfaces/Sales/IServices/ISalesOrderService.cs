using StockFlow.Application.DTOs.Sales.SalesOrder;
using StockFlow.Domain.Enums.Sales;

namespace StockFlow.Application.Interfaces.Sales.IServices
{
    public interface ISalesOrderService
    {
        Task<SalesOrderResponseDto>CreateAsync(CreateSalesOrderDto dto);

        Task<SalesOrderResponseDto>UpdateAsync(int id,UpdateSalesOrderDto dto);

        Task<SalesOrderResponseDto>GetByIdAsync(int id);

        Task<IEnumerable<SalesOrderResponseDto>>GetAllAsync();

        Task<IEnumerable<SalesOrderResponseDto>>GetByCustomerAsync(int customerId);

        Task<IEnumerable<SalesOrderResponseDto>>GetByStatusAsync(SalesOrderStatus status);

        Task UpdateStatusAsync(int orderId, UpdateSalesOrderStatusDto dto);

        Task CancelAsync(int orderId,string reason);
    }
}
