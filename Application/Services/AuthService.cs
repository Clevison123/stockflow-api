using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StockFlow.API.Application.DTOs.Auth;
using StockFlow.API.Application.Interfaces;
using StockFlow.API.Domain.Constants;
using StockFlow.API.Domain.Entities;
using StockFlow.API.Domain.Enums;
using StockFlow.API.DTOs.Auth;
using StockFlow.API.DTOs.Token;
using StockFlow.API.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StockFlow.API.Application.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IAuditService _auditService;

        public AuthService(
            AppDbContext context,
            IConfiguration configuration,
            IAuditService auditService)
        {
            _context = context;
            _configuration = configuration;
            _auditService = auditService;
        }

        // REGISTER
        public async Task<User?> RegisterAsync(RegisterDto dto)
        {
            // Verifica se email já existe
            var userExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);

            if (userExists)
                throw new Exception("Email already exists.");

            // Verifica se as senhas são iguais
            if (dto.Password != dto.ConfirmPassword)
                throw new Exception("Passwords do not match.");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),

                // NUNCA deixe o usuário escolher role direto
                Role = UserRole.Cashier
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            await _auditService.LogAsync(new AuditLog
            {
                UserId = user.Id,
                UserEmail = user.Email,
                Action = "REGISTER",
                EntityName = "User",
                EntityId = user.Id.ToString()
            });

            return user;
        }

        // LOGIN
        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            // salva HASH do refresh token
            user.RefreshToken = BCrypt.Net.BCrypt.HashPassword(refreshToken);

            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();

            await _auditService.LogAsync(new AuditLog
            {
                UserId = user.Id,
                UserEmail = user.Email,
                Action = "LOGIN",
                EntityName = "User",
                EntityId = user.Id.ToString()
            });

            return new AuthResponseDto
            {
                Token = jwtToken,
                RefreshToken = refreshToken
            };
        }

        // REFRESH TOKEN
        public async Task<AuthResponseDto?> RefreshTokenAsync(TokenDto dto)
        {
            var users = await _context.Users
                .Where(u => u.RefreshToken != null)
                .ToListAsync();

            var user = users.FirstOrDefault(u =>
                BCrypt.Net.BCrypt.Verify(dto.RefreshToken, u.RefreshToken));

            // verifica se usuário existe
            if (user == null)
                return null;

            // verifica se refresh token expirou
            if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
                return null;

            // gera novo JWT
            var newJwt = GenerateJwtToken(user);

            // gera novo refresh token
            var newRefreshToken = GenerateRefreshToken();

            // salva HASH do novo refresh token
            user.RefreshToken = BCrypt.Net.BCrypt.HashPassword(newRefreshToken);

            // nova expiração
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();

            await _auditService.LogAsync(new AuditLog
            {
                UserId = user.Id,
                UserEmail = user.Email,
                Action = "REFRESH_TOKEN",
                EntityName = "User",
                EntityId = user.Id.ToString()
            });

            return new AuthResponseDto
            {
                Token = newJwt,
                RefreshToken = newRefreshToken
            };
        }

        // JWT
        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),

                new Claim(CustomClaims.UserId, user.Id.ToString()),
                new Claim(CustomClaims.FullName, user.FullName),

                //importante em produção
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // REFRESH TOKEN GERADOR
        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}