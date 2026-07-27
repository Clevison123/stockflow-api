using StockFlow.Domain.Entities.Quality;
using StockFlow.Domain.Enums.Purchasing;

namespace StockFlow.Application.Interfaces.Quality
{
    public interface ISupplierClaimRepository
    {
        // READ
        Task<IEnumerable<SupplierClaim>> GetAllAsync();

        Task<SupplierClaim?> GetByIdAsync(int id);

        Task<IEnumerable<SupplierClaim>> GetBySupplierIdAsync(int supplierId);

        Task<IEnumerable<SupplierClaim>> GetByUserIdAsync(int userId);

        Task<IEnumerable<SupplierClaim>> GetOpenClaimsAsync();

        Task<IEnumerable<SupplierClaim>> GetResolvedClaimsAsync();

        Task<IEnumerable<SupplierClaim>> GetByTypeAsync(SupplierClaimType type);

        Task<IEnumerable<SupplierClaim>> GetByQualityIssueIdAsync(int qualityIssueId);

        // WRITE
        Task AddAsync(SupplierClaim claim);

        Task UpdateAsync(SupplierClaim claim);

        Task DeleteAsync(SupplierClaim claim);
    }
}