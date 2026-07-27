using StockFlow.Domain.Entities.Quality;
using StockFlow.Domain.Enums.Quality;

namespace StockFlow.Application.Interfaces.Quality
{
    public interface IQualityIssueRepository
    {
        // READ
        Task<IEnumerable<QualityIssue>> GetAllAsync();

        Task<QualityIssue?> GetByIdAsync(int id);

        Task<IEnumerable<QualityIssue>> GetByProductItemIdAsync(int productItemId);

        Task<IEnumerable<QualityIssue>> GetByUserIdAsync(int userId);

        Task<IEnumerable<QualityIssue>> GetOpenIssuesAsync();

        Task<IEnumerable<QualityIssue>> GetResolvedIssuesAsync();

        Task<IEnumerable<QualityIssue>> GetPendingSupplierClaimsAsync();

        Task<IEnumerable<QualityIssue>> GetByTypeAsync(QualityIssueType type);

        // WRITE
        Task AddAsync(QualityIssue issue);

        Task UpdateAsync(QualityIssue issue);

        Task DeleteAsync(QualityIssue issue);
    }
}