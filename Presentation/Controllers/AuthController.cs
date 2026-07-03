using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.API.src.Presentation.Responses;
using StockFlow.Application.DTOs.Identity.Token;
using StockFlow.Application.Interfaces.Identity;
using StockFlow.Application.Services.Indentity;

namespace StockFlow.API.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Auth")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ICurrentUserService _currentUserService;

        public AuthController(AuthService authService, ICurrentUserService currentUserService)
        {
            _authService = authService;
            _currentUserService = currentUserService;
        }

        // REGISTER
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RefreshTokenRequestDto dto)
        {
            var user = await _authService.RegisterAsync(dto);

            if (user == null)
                return BadRequest(ApiResponse<string>.ErrorResponse("Email already exists."));

            return StatusCode(201, ApiResponse<object>.SuccessResponse(new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.Role
            }, "User registered successfully."));
        }

        // LOGIN
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result == null)
                return Unauthorized(ApiResponse<string>.ErrorResponse("Invalid credentials."));

            return Ok(ApiResponse<object>.SuccessResponse(result, "Login successful."));
        }

        // REFRESH
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);

            if (result == null)
                return Unauthorized(ApiResponse<string>.ErrorResponse("Invalid refresh token."));

            return Ok(ApiResponse<object>.SuccessResponse(result, "Token refreshed."));
        }

        // ME
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(ApiResponse<object>.SuccessResponse(new
            {
                _currentUserService.UserId,
                _currentUserService.FullName,
                _currentUserService.Email,
                _currentUserService.Role
            }, "User data."));
        }
    }
}