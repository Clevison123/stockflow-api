using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StockFlow.Application.DTOs.Identity.Auth;
using StockFlow.Application.DTOs.Identity.Token;
using StockFlow.Application.Exceptions;
using StockFlow.Application.Interfaces.Identity;
using StockFlow.Application.Interfaces.Identity.IRepositories;
using StockFlow.Application.Interfaces.Identity.IServices;
using StockFlow.Domain.Entities.Identity;

namespace StockFlow.Application.Services.Indentity
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IJwtService jwtService,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        // LOGIN
        public async Task<LoginResponseDto> LoginAsync(
            LoginDto dto)
        {
            // Validate request
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            // Find user by email
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            // User not found
            if (user == null)
            {
                throw new UnauthorizedException(
                    "Email ou senha inválidos.");
            }

            // User account is inactive
            if (!user.IsActive)
            {
                throw new ForbiddenException(
                    "Usuário inativo.");
            }

            // Verify password
            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password);

            // Invalid password
            if (result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedException(
                    "Email ou senha inválidos.");
            }

            // Generate Access Token (JWT)
            var accessToken = _jwtService.GenerateAccessToken(user);

            // Generate a new Refresh Token
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Associate the Refresh Token with the authenticated user
            refreshToken.UserId = user.Id;

            // Save the Refresh Token in the database
            await _refreshTokenRepository.AddAsync(refreshToken);

            // Update the user's last successful login
            user.LastLoginAt = DateTime.UtcNow;

            // Persist the updated user
            await _userRepository.UpdateAsync(user);

            // Return authentication data to the client
            return new LoginResponseDto
            {
                AccessToken = accessToken,

                RefreshToken = refreshToken.Token,

                // TODO:
                // For now this returns the Refresh Token expiration.
                // In the future this should return the Access Token expiration
                // from JwtService to better reflect JWT lifetime.
                ExpiresAt = refreshToken.ExpiresAt
            };
        }

        // REGISTER
        public async Task RegisterAsync(
            RegisterDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                throw new BusinessRuleException(
                    "As senhas não coincidem.");
            }

            var existingUser =
                await _userRepository.GetByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                throw new ConflictException(
                    "Email já está em uso.");
            }

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                IsActive = true
            };

            user.PasswordHash =
                _passwordHasher.HashPassword(
                    user,
                    dto.Password);

            await _userRepository.AddAsync(user);
        }

        // REFRESH TOKEN
        public async Task<RefreshTokenResponseDto> RefreshTokenAsync(
            RefreshTokenRequestDto dto)
        {
            // Validate request
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            // Find the refresh token in the database
            var refreshToken = await _refreshTokenRepository
                .GetByTokenAsync(dto.RefreshToken);

            // Refresh token not found
            if (refreshToken == null)
            {
                throw new UnauthorizedAccessException(
                    "Refresh Token inválido.");
            }

            // Check if the refresh token has expired
            if (refreshToken.ExpiresAt <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException(
                    "Refresh Token expirado.");
            }

            // Check if the refresh token has already been revoked
            if (refreshToken.IsRevoked)
            {
                throw new UnauthorizedAccessException(
                    "Refresh Token revogado.");
            }

            // Find the user associated with the refresh token
            var user = await _userRepository
                .GetByIdAsync(refreshToken.UserId);

            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    "Usuário não encontrado.");
            }

            // Generate a new Access Token
            var accessToken =
                _jwtService.GenerateAccessToken(user);

            // Generate a new Refresh Token (Entity)
            var newRefreshToken =
                _jwtService.GenerateRefreshToken();

            // Associate the new Refresh Token with the user
            newRefreshToken.UserId = user.Id;

            // Revoke the old Refresh Token
            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;

            // Update the old Refresh Token
            await _refreshTokenRepository.UpdateAsync(refreshToken);

            // Save the new Refresh Token
            await _refreshTokenRepository.AddAsync(newRefreshToken);

            // Return the new tokens to the client
            return new RefreshTokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token,
                ExpiresAt = newRefreshToken.ExpiresAt
            };
        }

        // LOGOUT
        public async Task LogoutAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new ArgumentException("Refresh Token é obrigatório.");

            var currentRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (currentRefreshToken == null)
                throw new UnauthorizedAccessException("Refresh Token inválido.");

            if (currentRefreshToken.ExpiresAt <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh Token expirado.");

            if (currentRefreshToken.IsRevoked)
                throw new UnauthorizedAccessException("Refresh Token já revogado.");

            currentRefreshToken.IsRevoked = true;
            currentRefreshToken.RevokedAt = DateTime.UtcNow;

            await _refreshTokenRepository.UpdateAsync(currentRefreshToken);
        }
    }
}
