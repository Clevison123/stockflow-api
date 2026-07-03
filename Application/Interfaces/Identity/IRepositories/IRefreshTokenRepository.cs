using StockFlow.Domain.Entities.Identity;

namespace StockFlow.Application.Interfaces.Identity.IRepositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token);

        Task<RefreshToken?> GetByTokenAsync(string token);

        Task UpdateAsync(RefreshToken token);
    }
}
