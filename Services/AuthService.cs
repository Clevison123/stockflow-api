using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StockFlow.API.Constants;
using StockFlow.API.Data;
using StockFlow.API.DTOs.Auth;
using StockFlow.API.Entities;
using StockFlow.API.Enums;
using StockFlow.API.Interfaces; 
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StockFlow.API.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        
        private readonly IAuditService _auditService;

        public AuthService(
            AppDbContext context,
            IConfiguration configuration,
            IAuditService auditService 
        )
        {
            _context = context;
            _configuration = configuration;
            _auditService = auditService; 
        }

        public async Task<User?> RegisterAsync(RegisterDto dto)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);

            if (userExists)
                return null;

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
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
                EntityId = user.Id.ToString(),
                NewValues = System.Text.Json.JsonSerializer.Serialize(new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    user.Role
                })
            });

            return user;
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return null;

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!isPasswordValid)
                return null;

            var token = GenerateJwtToken(user);

            
            await _auditService.LogAsync(new AuditLog
            {
                UserId = user.Id,
                UserEmail = user.Email,
                Action = "LOGIN",
                EntityName = "User",
                EntityId = user.Id.ToString()
            });

            return token;
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),

                
                new Claim(CustomClaims.UserId, user.Id.ToString()),
                new Claim(CustomClaims.FullName, user.FullName)
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
    }
}