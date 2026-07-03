using StockFlow.Domain.Entities.Identity;

namespace StockFlow.Application.Interfaces.Identity
{
    public interface IUserRepository
    {
        // READ
        Task<IEnumerable<User>> GetAllAsync(string? search);

        Task<User?> GetByIdAsync(int id);

        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByEmployeeCodeAsync(string employeeCode);

        // CHECKS (baseado em campos da entity)
        Task<bool> EmailExistsAsync(string email, int? ignoreId = null);

        Task<bool> EmployeeCodeExistsAsync(string employeeCode, int? ignoreId = null);

        // WRITE
        Task AddAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(User user);

        // OPTIONAL (por causa da Entity)
        Task AddRefreshTokenAsync(int userId, RefreshToken token);

        Task<User?> GetWithRefreshTokensAsync(int id);

        Task<User?> GetByRefreshTokenAsync( string token);
    }
}