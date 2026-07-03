using StockFlow.Application.DTOs.Quality.QualityIssue;

namespace StockFlow.Application.Interfaces.Quality.IServices
{
    public interface IQualityIssueService
    {
        Task<QualityIssueResponseDto>
            CreateAsync(
                CreateQualityIssueDto dto);

        Task<QualityIssueResponseDto>
            UpdateAsync(
                int id,
                UpdateQualityIssueDto dto);

        Task<QualityIssueResponseDto>
            GetByIdAsync(
                int id);

        Task<IEnumerable<QualityIssueResponseDto>>
            GetAllAsync();

        Task<IEnumerable<QualityIssueResponseDto>>
            GetByProductItemAsync(
                int productItemId);

        Task<IEnumerable<QualityIssueResponseDto>>
            GetOpenIssuesAsync();

        Task<IEnumerable<QualityIssueResponseDto>>
            GetResolvedIssuesAsync();

        Task<IEnumerable<QualityIssueResponseDto>>
            GetPendingSupplierClaimsAsync();

        Task ResolveAsync(
            int issueId,
            ResolveQualityIssueDto dto);
    }
}
