using StockFlow.Application.DTOs.Identity.Auth;
using StockFlow.Application.DTOs.Identity.Token;

namespace StockFlow.Application.Interfaces.Identity
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto dto);

        Task RegisterAsync(RegisterDto dto);

        Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto);

        // TODO:
        // The logout operation currently accepts only the refresh token, as it is
        // sufficient to identify and revoke the user's session.
        //
        // In the future, consider replacing this parameter with a LogoutRequestDto
        // if additional metadata (e.g., IP address, User-Agent, device information,
        // or audit data) needs to be captured.
        Task LogoutAsync(string refreshToken);
    }
}