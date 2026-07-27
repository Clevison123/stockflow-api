using StockFlow.Domain.Enums.Identity;

namespace StockFlow.Application.DTOs.Identity.Users
{
    public class UpdateUserDto
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public UserRole Role { get; set; }
    }
}
