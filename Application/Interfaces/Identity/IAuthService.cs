using StockFlow.Application.DTOs.Identity.Auth;
using StockFlow.Application.DTOs.Identity.Token;

namespace StockFlow.Application.Interfaces.Identity
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(
            LoginDto dto);

        Task RegisterAsync(
            RegisterDto dto);

        Task<RefreshTokenResponseDto> RefreshTokenAsync(
            RefreshTokenRequestDto dto);

        Task LogoutAsync(
            int userId);
    }
}