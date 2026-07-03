using StockFlow.Application.DTOs.Sales.Customers;

namespace StockFlow.Application.Interfaces.Sales.IServices
{
    public interface ICustomerService
    {
        Task<CustomerResponseDto>
            CreateAsync(
                CreateCustomerDto dto);

        Task<CustomerResponseDto>
            UpdateAsync(
                int id,
                UpdateCustomerDto dto);

        Task<CustomerResponseDto>
            GetByIdAsync(
                int id);

        Task<IEnumerable<CustomerResponseDto>>
            GetAllAsync();

        Task<CustomerResponseDto>
            GetByCnpjAsync(
                string cnpj);

        Task ActivateAsync(
            int id);

        Task DeactivateAsync(
            int id);
    }
}
