using StockFlow.Domain.Entities.Sales;

namespace StockFlow.Application.Interfaces.Sales
{
    public interface ICustomerRepository
    {
        // READ
        Task<IEnumerable<Customer>> GetAllAsync(string? search);

        Task<Customer?> GetByIdAsync(int id);

        Task<Customer?> GetByEmailAsync(string email);

        Task<Customer?> GetByCnpjAsync(string cnpj);

        Task<bool> EmailExistsAsync(string email, int? ignoreId = null);

        Task<bool> CnpjExistsAsync(string cnpj, int? ignoreId = null);

        // RELATIONS
        Task<IEnumerable<Customer>> GetWithSalesOrdersAsync(int customerId);

        Task<IEnumerable<Customer>> GetWithDeliveriesAsync(int customerId);

        // WRITE
        Task AddAsync(Customer customer);

        Task UpdateAsync(Customer customer);

        Task DeleteAsync(Customer customer);
    }
}