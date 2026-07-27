using StockFlow.Domain.Entities.Quality;
using StockFlow.Domain.Enums.Sales;

namespace StockFlow.Application.Interfaces.Quality
{
    public interface ICustomerClaimRepository
    {
        // READ

        Task<IEnumerable<CustomerClaim>> GetAllAsync();
        Task<CustomerClaim?> GetByIdAsync(int id);
        Task<IEnumerable<CustomerClaim>> GetByCustomerIdAsync(int customerId);

        Task<IEnumerable<CustomerClaim>> GetBySalesOrderIdAsync( int salesOrderId);

        Task<IEnumerable<CustomerClaim>>GetOpenClaimsAsync();

        Task<IEnumerable<CustomerClaim>>GetResolvedClaimsAsync();

        Task<IEnumerable<CustomerClaim>> GetByTypeAsync( CustomerClaimType type);

        // WRITE

        Task AddAsync( CustomerClaim claim);

        Task UpdateAsync( CustomerClaim claim);

        Task DeleteAsync( CustomerClaim claim);
    }
}