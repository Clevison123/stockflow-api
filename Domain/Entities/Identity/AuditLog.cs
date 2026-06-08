using StockFlow.Domain.Entities.Common;
using StockFlow.Domain.Enums;

namespace StockFlow.Domain.Entities.Identity
{
    public class AuditLog : BaseEntity
    {
        public int? UserId { get; set; }

        public User? User { get; set; }

        public string UserEmail { get; set; } = string.Empty;

        public AuditAction Action { get; set; }

        public string EntityName { get; set; } = string.Empty;

        public string EntityId { get; set; } = string.Empty;

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }
    }
}
