namespace StockFlow.Domain.Enums.Audit
{
    public enum AuditAction
    {
        Create = 1,
        Update = 2,
        Delete = 3,
        Activate = 4,
        Deactivate = 5,
        Login = 6,
        Logout = 7,
        RefreshToken = 8,
        Approve = 9,
        Reject = 10,
        Restore = 11,
        Export = 12,
        Import = 13
    }
}