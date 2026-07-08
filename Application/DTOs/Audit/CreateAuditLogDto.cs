using StockFlow.Domain.Enums;

namespace StockFlow.Application.DTOs.Audit
{
    public class CreateAuditLogDto
    {
        //public int? UserId { get; set; }

        public AuditAction Action { get; set; }

        public AuditEntity Entity { get; set; }

        public string? EntityId { get; set; }

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }

        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }

        //public DateTime CreatedAt { get; set; }
    }
}
