using StockFlow.Domain.Enums.Identity;

namespace StockFlow.Application.DTOs.Identity.Users
{
    public class ChangeUserRoleDto
    {
        public UserRole Role { get; set; }

        public string Reason { get; set; } = string.Empty;
    }
}