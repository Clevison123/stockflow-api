using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using StockFlow.API.Application.DTOs.Users;
using StockFlow.API.Application.Exceptions;
using StockFlow.API.Application.Interfaces;
using StockFlow.API.Domain.Entities;
using StockFlow.API.Infrastructure.Data;

namespace StockFlow.API.Application.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL USERS
        public async Task<IEnumerable<UserResponseDto>> GetAllAsync(
            string? search)
        {
            var query = _context.Users.AsQueryable();

            // SEARCH
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search
                    .Trim()
                    .ToLower();

                query = query.Where(u =>
                    u.FullName.ToLower().Contains(search) ||
                    u.Email.ToLower().Contains(search));
            }

            var users = await query
                .Select(user => MapToResponse(user))
                .ToListAsync();

            return users;
        }

        // GET USER BY ID
        public async Task<UserResponseDto> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
            {
                throw new NotFoundException(
                    "User not found");
            }

            return MapToResponse(user);
        }

        // CREATE USER
        public async Task<UserResponseDto> CreateAsync(
            CreateUserDto dto)
        {
            var normalizedEmail = dto.Email
                .Trim()
                .ToLower();

            var emailAlreadyExists = await _context.Users
                .AnyAsync(u =>
                    u.Email.ToLower() == normalizedEmail);

            if (emailAlreadyExists)
            {
                throw new ConflictException(
                    "Email already exists");
            }

            var passwordHash =
                BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                FullName = dto.FullName.Trim(),
                Email = normalizedEmail,
                PasswordHash = passwordHash,
                Role = dto.Role,
                IsActive = true
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return MapToResponse(user);
        }

        // UPDATE USER
        public async Task<UserResponseDto> UpdateAsync(
            int id,
            UpdateUserDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
            {
                throw new NotFoundException(
                    "User not found");
            }

            var normalizedEmail = dto.Email
                .Trim()
                .ToLower();

            var emailAlreadyExists = await _context.Users
                .AnyAsync(u =>
                    u.Email.ToLower() == normalizedEmail &&
                    u.Id != id);

            if (emailAlreadyExists)
            {
                throw new ConflictException(
                    "Email already exists");
            }

            user.FullName = dto.FullName.Trim();
            user.Email = normalizedEmail;
            user.Role = dto.Role;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToResponse(user);
        }

        // DEACTIVATE USER
        public async Task DeactivateAsync(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
            {
                throw new NotFoundException(
                    "User not found");
            }

            if (!user.IsActive)
            {
                throw new BusinessRuleException(
                    "User is already deactivated");
            }

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        // ACTIVATE USER
        public async Task ActivateAsync(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
            {
                throw new NotFoundException(
                    "User not found");
            }

            if (user.IsActive)
            {
                throw new BusinessRuleException(
                    "User is already active");
            }

            user.IsActive = true;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        // MAP ENTITY TO DTO
        private static UserResponseDto MapToResponse(
            User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}