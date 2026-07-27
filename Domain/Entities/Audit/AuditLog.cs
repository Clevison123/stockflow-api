using StockFlow.Domain.Entities.Common;
using StockFlow.Domain.Entities.Identity;
using StockFlow.Domain.Enums.Audit;

namespace StockFlow.Domain.Entities.Audit
{
    public class AuditLog : BaseEntity
    {
        public int? UserId { get; set; }

        public User? User { get; set; }

        public string UserEmail { get; set; } = string.Empty;

        public AuditAction Action { get; set; }

        public AuditEntity Entity { get; set; }

        public string EntityId { get; set; } = string.Empty;

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }

        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }
    }
}
