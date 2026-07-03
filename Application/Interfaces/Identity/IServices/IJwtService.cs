using StockFlow.Domain.Entities.Identity;

namespace StockFlow.Application.Interfaces.Identity.IServices
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);

        RefreshToken GenerateRefreshToken();
    }
}
