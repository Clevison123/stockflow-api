using StockFlow.Domain.Entities.Common;

namespace StockFlow.Domain.Entities.Identity
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime? RevokedAt { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;
    }
}