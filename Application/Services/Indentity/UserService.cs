using Microsoft.AspNetCore.Identity;
using StockFlow.Application.DTOs.Identity.Users;
using StockFlow.Application.Exceptions;
using StockFlow.Application.Interfaces.Identity;
using StockFlow.Domain.Entities.Identity;
using StockFlow.Domain.Enums.Identity;

namespace StockFlow.Application.Services.Indentity
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        // GET ALL
        public async Task<IEnumerable<UserResponseDto>> GetAllAsync(string? search)
        {
            var users = await _userRepository.GetAllAsync(search);

            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.Phone,
                EmployeeCode = u.EmployeeCode,
                Role = u.Role,
                IsActive = u.IsActive,
                LastLoginAt = u.LastLoginAt,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            });
        }

        // GET BY ID
        public async Task<UserResponseDto> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new NotFoundException("Usuário não encontrado.");

            return Map(user);
        }

        // CREATE
        public async Task<UserResponseDto> CreateAsync(CreateUserDto dto)
        {
            var emailExists = await _userRepository.EmailExistsAsync(dto.Email);

            if (emailExists)
                throw new ConflictException("Email já está em uso.");

            var employeeCodeExists = await _userRepository.EmployeeCodeExistsAsync(dto.EmployeeCode);

            if (employeeCodeExists)
                throw new ConflictException("Código de funcionário já existe.");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                EmployeeCode = dto.EmployeeCode,
                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _userRepository.AddAsync(user);

            return Map(user);
        }

        // UPDATE
        public async Task<UserResponseDto> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new NotFoundException("Usuário não encontrado.");

            var emailExists = await _userRepository.EmailExistsAsync(dto.Email, id);

            if (emailExists)
                throw new ConflictException("Email já está em uso.");

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.Role = dto.Role;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            return Map(user);
        }

        // ACTIVATE
        public async Task ActivateAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new NotFoundException("Usuário não encontrado.");

            user.IsActive = true;
            await _userRepository.UpdateAsync(user);
        }

        // DEACTIVATE
        public async Task DeactivateAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new NotFoundException("Usuário não encontrado.");

            user.IsActive = false;
            await _userRepository.UpdateAsync(user);
        }

        // CHANGE ROLE
        public async Task ChangeRoleAsync(int userId, ChangeUserRoleDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("Usuário não encontrado.");

            if (user.Role == UserRole.Administrator && dto.Role != UserRole.Administrator)
                throw new BusinessRuleException("Não é permitido rebaixar um administrador diretamente.");

            user.Role = dto.Role;
            await _userRepository.UpdateAsync(user);
        }

        // RESET PASSWORD
        public async Task ResetPasswordAsync(int userId, ResetPasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("Usuário não encontrado.");

            _passwordHasher.HashPassword(user, dto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
        }

        // MAPPER
        private static UserResponseDto Map(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                EmployeeCode = user.EmployeeCode,
                Role = user.Role,
                IsActive = user.IsActive,
                LastLoginAt = user.LastLoginAt,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}