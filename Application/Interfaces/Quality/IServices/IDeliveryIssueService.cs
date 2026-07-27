using StockFlow.Application.DTOs.Quality.DeliveryIssue;

namespace StockFlow.Application.Interfaces
{
    public interface IDeliveryIssueService
    {
        Task<DeliveryIssueResponseDto> CreateAsync(CreateDeliveryIssueDto dto);

        Task<DeliveryIssueResponseDto> UpdateAsync( int id, UpdateDeliveryIssueDto dto);

        Task<DeliveryIssueResponseDto> GetByIdAsync(int id);

        Task<IEnumerable<DeliveryIssueResponseDto>> GetAllAsync();

        Task<IEnumerable<DeliveryIssueResponseDto>> GetByDeliveryAsync( int deliveryId);

        Task<IEnumerable<DeliveryIssueResponseDto>> GetOpenIssuesAsync();

        Task<IEnumerable<DeliveryIssueResponseDto>> GetResolvedIssuesAsync();

        Task ResolveAsync( int issueId, ResolveDeliveryIssueDto dto);
    }
}
