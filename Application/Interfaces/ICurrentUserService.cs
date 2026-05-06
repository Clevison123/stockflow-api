namespace StockFlow.API.Application.Interfaces
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string? FullName { get; }
        string? Email { get; }
        string? Role { get; }
        bool IsAuthenticated { get; }
    }
}
