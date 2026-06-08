using StockFlow.Application.DTOs.Quality.CustomerClaim;

namespace StockFlow.Application.Interfaces.Quality
{
    public interface ICustomerClaimService
    {
        Task<CustomerClaimResponseDto>
            CreateAsync(
                CreateCustomerClaimDto dto);

        Task<CustomerClaimResponseDto>
            UpdateAsync(
                int id,
                UpdateCustomerClaimDto dto);

        Task<CustomerClaimResponseDto>
            GetByIdAsync(
                int id);

        Task<IEnumerable<CustomerClaimResponseDto>>
            GetAllAsync();

        Task<IEnumerable<CustomerClaimResponseDto>>
            GetByCustomerAsync(
                int customerId);

        Task<IEnumerable<CustomerClaimResponseDto>>
            GetBySalesOrderAsync(
                int salesOrderId);

        Task<IEnumerable<CustomerClaimResponseDto>>
            GetOpenClaimsAsync();

        Task<IEnumerable<CustomerClaimResponseDto>>
            GetResolvedClaimsAsync();

        Task ResolveAsync(
            int claimId,
            ResolveCustomerClaimDto dto);
    }
}
