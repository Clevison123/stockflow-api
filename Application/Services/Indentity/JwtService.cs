using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StockFlow.Application.Interfaces.Identity.IServices;
using StockFlow.Application.Settings;
using StockFlow.Domain.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StockFlow.Application.Services.Indentity
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(
            IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateAccessToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var key =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _jwtSettings.Key));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()),
                new Claim(
                    ClaimTypes.Name,
                    user.FullName),
                new Claim(
                    ClaimTypes.Email,
                    user.Email),
                new Claim(
                    ClaimTypes.Role,
                    user.Role.ToString())
            };

            var token =
                new JwtSecurityToken(
                    issuer:
                        _jwtSettings.Issuer,

                    audience:
                        _jwtSettings.Audience,
                    claims:
                        claims,
                    expires:
                       DateTime.UtcNow.AddHours(
                           _jwtSettings.AccessTokenExpirationHours),
                    signingCredentials:
                        credentials);
            return new JwtSecurityTokenHandler()
                .WriteToken(token); 
        }

        public RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(
                    RandomNumberGenerator.GetBytes(64)),

                ExpiresAt = DateTime.UtcNow.AddDays(
                    _jwtSettings.RefreshTokenExpirationDays),

                IsRevoked = false,

                RevokedAt = null
            };
        }
    }
}
