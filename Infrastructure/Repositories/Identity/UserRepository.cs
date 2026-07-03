using Microsoft.EntityFrameworkCore;
using StockFlow.Application.Interfaces.Identity;
using StockFlow.Domain.Entities.Identity;
using StockFlow.Infrastructure.Data;

namespace StockFlow.Infrastructure.Repositories.Identity
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(
            AppDbContext context)
        {
            _context = context;
        }

        // READ

        public async Task<IEnumerable<User>> GetAllAsync(
            string? search)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.FullName.Contains(search));
            }

            return await query.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(
            int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetByEmailAsync(
            string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetByEmployeeCodeAsync(
            string employeeCode)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                    x => x.EmployeeCode == employeeCode);
        }

        // CHECKS

        public async Task<bool> EmailExistsAsync(
            string email,
            int? ignoreId = null)
        {
            return await _context.Users.AnyAsync(x =>
                x.Email == email &&
                (!ignoreId.HasValue ||
                 x.Id != ignoreId.Value));
        }

        public async Task<bool> EmployeeCodeExistsAsync(
            string employeeCode,
            int? ignoreId = null)
        {
            return await _context.Users.AnyAsync(x =>
                x.EmployeeCode == employeeCode &&
                (!ignoreId.HasValue ||
                 x.Id != ignoreId.Value));
        }

        // WRITE

        public async Task AddAsync(
            User user)
        {
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(
            User user)
        {
            _context.Users.Update(user);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(
            User user)
        {
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }

        // REFRESH TOKENS

        public async Task AddRefreshTokenAsync(
            int userId,
            RefreshToken token)
        {
            token.UserId = userId;

            await _context.RefreshTokens
                .AddAsync(token);

            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetWithRefreshTokensAsync(
            int id)
        {
            return await _context.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetByRefreshTokenAsync(
            string token)
        {
            return await _context.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(x =>
                    x.RefreshTokens.Any(rt =>
                        rt.Token == token &&
                        !rt.IsRevoked &&
                        rt.ExpiresAt > DateTime.UtcNow));
        }
    }
}