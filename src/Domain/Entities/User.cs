using StockFlow.API.Domain.Constants;
using StockFlow.API.src.Domain.Enums;

namespace StockFlow.API.src.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Cashier;

        public bool IsActive { get; set; } = true;

        // para o token
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
