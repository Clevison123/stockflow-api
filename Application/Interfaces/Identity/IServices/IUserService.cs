using StockFlow.Application.DTOs.Identity.Users;
using StockFlow.Domain.Enums;

namespace StockFlow.Application.Interfaces.Identity
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllAsync(
            string? search);

        Task<UserResponseDto> GetByIdAsync(
            int id);

        Task<UserResponseDto> CreateAsync(
            CreateUserDto dto);

        Task<UserResponseDto> UpdateAsync(
            int id,
            UpdateUserDto dto);

        Task ActivateAsync(
            int id);

        Task DeactivateAsync(
            int id);

        Task ChangeRoleAsync(
            int userId,
            ChangeUserRoleDto dto);

        Task ResetPasswordAsync(
            int userId,
            ResetPasswordDto dto);
    }
}