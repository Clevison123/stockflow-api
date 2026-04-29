namespace StockFlow.API.Interfaces
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
