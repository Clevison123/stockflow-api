using StockFlow.Application.Interfaces.Identity;
using StockFlow.Domain.Enums.Identity;
using System.Security.Claims;

namespace StockFlow.Application.Services.Indentity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?
                    .User
                    .FindFirst(ClaimTypes.NameIdentifier)?
                    .Value;

                if (int.TryParse(userIdClaim, out int userId))
                    return userId;

                return null;
            }
        }

        public string? FullName =>
            _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.Name)?
                .Value;

        public string? Email =>
            _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.Email)?
                .Value;

        public UserRole? Role
        {
            get
            {
                var role = _httpContextAccessor.HttpContext?
                    .User
                    .FindFirst(ClaimTypes.Role)?
                    .Value;

                if (Enum.TryParse<UserRole>(role, out var userRole))
                    return userRole;

                return null;
            }
        }

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }
}