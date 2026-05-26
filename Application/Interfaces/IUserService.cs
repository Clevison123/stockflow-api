using StockFlow.API.Application.DTOs.Users;

namespace StockFlow.API.Application.Interfaces
{
    public interface IUserService
    {
        // GET ALL USERS
        Task<IEnumerable<UserResponseDto>> GetAllAsync(
            string? search);

        // GET USER BY ID
        Task<UserResponseDto> GetByIdAsync(int id);

        // CREATE USER
        Task<UserResponseDto> CreateAsync(
            CreateUserDto dto);

        // UPDATE USER
        Task<UserResponseDto> UpdateAsync(
            int id,
            UpdateUserDto dto);

        // DEACTIVATE USER
        Task DeactivateAsync(int id);

        // ACTIVATE USER
        Task ActivateAsync(int id);
    }
}