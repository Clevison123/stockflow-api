using StockFlow.Domain.Enums;

namespace StockFlow.Application.Interfaces.Identity
{
    public interface ICurrentUserService
    {
        int? UserId { get; }

        string? FullName { get; }

        string? Email { get; }

        UserRole? Role { get; }

        bool IsAuthenticated { get; }
    }
}