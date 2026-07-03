using StockFlow.Application.DTOs.SupplierClaim;

namespace StockFlow.Application.Interfaces.Quality
{
    public interface ISupplierClaimService
    {
        Task<SupplierClaimResponseDto>
            CreateAsync(
                CreateSupplierClaimDto dto);

        Task<SupplierClaimResponseDto>
            UpdateAsync(
                int id,
                UpdateSupplierClaimDto dto);

        Task<SupplierClaimResponseDto>
            GetByIdAsync(
                int id);

        Task<IEnumerable<SupplierClaimResponseDto>>
            GetAllAsync();

        Task<IEnumerable<SupplierClaimResponseDto>>
            GetBySupplierAsync(
                int supplierId);

        Task<IEnumerable<SupplierClaimResponseDto>>
            GetByQualityIssueAsync(
                int qualityIssueId);

        Task<IEnumerable<SupplierClaimResponseDto>>
            GetOpenClaimsAsync();

        Task<IEnumerable<SupplierClaimResponseDto>>
            GetResolvedClaimsAsync();

        Task ResolveAsync(
            int claimId,
            ResolveSupplierClaimDto dto);
    }
}
