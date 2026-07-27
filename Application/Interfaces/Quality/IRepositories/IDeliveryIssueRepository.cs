using StockFlow.Domain.Entities.Quality;
using StockFlow.Domain.Enums.Quality;

namespace StockFlow.Application.Interfaces.Quality
{
    public interface IDeliveryIssueRepository
    {
        // READ
        Task<IEnumerable<DeliveryIssue>> GetAllAsync();

        Task<DeliveryIssue?> GetByIdAsync(int id);

        Task<IEnumerable<DeliveryIssue>> GetByDeliveryIdAsync(int deliveryId);

        Task<IEnumerable<DeliveryIssue>> GetOpenIssuesAsync();

        Task<IEnumerable<DeliveryIssue>> GetResolvedIssuesAsync();

        Task<IEnumerable<DeliveryIssue>> GetByTypeAsync(DeliveryIssueType type);

        // WRITE
        Task AddAsync(DeliveryIssue issue);

        Task UpdateAsync(DeliveryIssue issue);

        Task DeleteAsync(DeliveryIssue issue);
    }
}