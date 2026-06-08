using StockFlow.Application.DTOs.Sales.SalesOrder;
using StockFlow.Domain.Enums;

namespace StockFlow.Application.Interfaces.Sales
{
    public interface ISalesOrderService
    {
        Task<SalesOrderResponseDto>
            CreateAsync(
                CreateSalesOrderDto dto);

        Task<SalesOrderResponseDto>
            UpdateAsync(
                int id,
                UpdateSalesOrderDto dto);

        Task<SalesOrderResponseDto>
            GetByIdAsync(
                int id);

        Task<IEnumerable<SalesOrderResponseDto>>
            GetAllAsync();

        Task<IEnumerable<SalesOrderResponseDto>>
            GetByCustomerAsync(
                int customerId);

        Task<IEnumerable<SalesOrderResponseDto>>
            GetByStatusAsync(
                SalesOrderStatus status);

        Task UpdateStatusAsync(
            int orderId,
            SalesOrderStatus status);

        Task CancelAsync(
            int orderId,
            string reason);
    }
}
