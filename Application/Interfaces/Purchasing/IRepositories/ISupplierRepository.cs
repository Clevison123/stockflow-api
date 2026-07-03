using StockFlow.Domain.Entities.Purchasing;

namespace StockFlow.Application.Interfaces.Purchasing
{
    public interface ISupplierRepository
    {
        // READ
        Task<IEnumerable<Supplier>> GetAllAsync(string? search);

        Task<Supplier?> GetByIdAsync(int id);

        Task<Supplier?> GetByEmailAsync(string email);

        Task<bool> EmailExistsAsync(string email, int? ignoreId = null);

        // BUSINESS
        Task<IEnumerable<Supplier>> GetActiveAsync();

        Task<IEnumerable<Supplier>> GetWithProductsAsync(int supplierId);

        // WRITE
        Task AddAsync(Supplier supplier);

        Task UpdateAsync(Supplier supplier);

        Task DeleteAsync(Supplier supplier);
    }
}