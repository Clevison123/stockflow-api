using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.API.Constants;
using StockFlow.API.DTOs.Auth;
using StockFlow.API.Interfaces;
using StockFlow.API.Models;
using StockFlow.API.Services;
using System.Security.Claims;
using StockFlow.API.Extensions;

namespace StockFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Auth")]
    [Authorize] //Protege tudo por padrão
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ICurrentUserService _currentUserService;

        public AuthController(AuthService authService, ICurrentUserService currentUserService)
        {
            _authService = authService;
            _currentUserService = currentUserService;
        }

        //Público
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _authService.RegisterAsync(dto);

            if (user == null)
            {
                var errorResponse = ApiResponse<string>.ErrorResponse("Email is already in use.");
                return BadRequest(errorResponse);
            }

            var response = ApiResponse<object>.SuccessResponse(new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.Role,
                user.CreatedAt
            }, "User registered successfully.");

            return StatusCode(StatusCodes.Status201Created, response); 
        }

        //Público
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.LoginAsync(dto);

            if (token == null)
            {
                var errorResponse = ApiResponse<string>.ErrorResponse("Invalid email or password.");
                return Unauthorized(errorResponse);
            }

            var response = ApiResponse<object>.SuccessResponse(new { token }, "Login successful.");
            return Ok(response);
        }

        //Usuário autenticado
        [HttpGet("me")]
        public IActionResult Me()
        {
            var userData = new
            {
                Id = _currentUserService.UserId,
                FullName = _currentUserService.FullName,
                Email = _currentUserService.Email,
                Role = _currentUserService.Role,
                IsAuthenticated = _currentUserService.IsAuthenticated
            };

            var response = ApiResponse<object>.SuccessResponse(userData, "Authenticated user retrieved successfully.");
            return Ok(response);
        }

        // Apenas Admin
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            var response = ApiResponse<string>.SuccessResponse("Only admins can access this route.", "Access granted.");
            return Ok(response);
        }

        //Usuário autenticado
        [HttpGet("my-id")]
        public IActionResult MyId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                var errorResponse = ApiResponse<string>.ErrorResponse("Invalid user ID.");
                return Unauthorized(errorResponse);
            }

            var response = ApiResponse<object>.SuccessResponse(new { userId }, "Authenticated user ID retrieved successfully.");
            return Ok(response);
        }

        //Usuário autenticado 
        [HttpGet("me-extension")]
        public IActionResult GetUserFromToken()
        {
            var data = new
            {
                UserId = User.GetUserId(),
                FullName = User.GetUserFullName(),
                Email = User.GetUserEmail(),
                Role = User.GetUserRole()
            };

            var response = ApiResponse<object>.SuccessResponse(data, "User data retrieved using extension methods.");
            return Ok(response);
        }
    }
}