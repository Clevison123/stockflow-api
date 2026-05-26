using StockFlow.API.Application.DTOs.Users;

namespace StockFlow.API.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllAsync();

        Task<UserResponseDto> GetByIdAsync(int id);

        Task<UserResponseDto> CreateAsync(CreateUserDto dto);

        Task<UserResponseDto> UpdateAsync(
            int id,
            UpdateUserDto dto);

        Task DeactivateAsync(int id);

        Task ActivateAsync(int id);
    }
}