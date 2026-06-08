using Microsoft.AspNetCore.Mvc;
using StockFlow.API.src.Presentation.Responses;
using StockFlow.Application.DTOs.Identity.Users;
using StockFlow.Application.Interfaces.Identity;

namespace StockFlow.API.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/users")]
    [ApiExplorerSettings(GroupName = "Users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search)
        {
            var users = await _userService.GetAllAsync(search);

            return Ok(
                ApiResponse<IEnumerable<UserResponseDto>>
                    .SuccessResponse(users)
            );
        }

        // GET: api/users/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            return Ok(
                ApiResponse<UserResponseDto>
                    .SuccessResponse(user)
            );
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateUserDto dto)
        {
            var createdUser = await _userService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdUser.Id },
                ApiResponse<UserResponseDto>
                    .SuccessResponse(
                        createdUser,
                        "User created successfully")
            );
        }

        // PUT: api/users/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateUserDto dto)
        {
            var updatedUser = await _userService.UpdateAsync(id, dto);

            return Ok(
                ApiResponse<UserResponseDto>
                    .SuccessResponse(
                        updatedUser,
                        "User updated successfully")
            );
        }

        // PATCH: api/users/1/deactivate
        [HttpPatch("{id:int}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            await _userService.DeactivateAsync(id);

            return Ok(
                ApiResponse<string>
                    .SuccessResponse(
                        "User deactivated successfully")
            );
        }

        // PATCH: api/users/1/activate
        [HttpPatch("{id:int}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            await _userService.ActivateAsync(id);

            return Ok(
                ApiResponse<string>
                    .SuccessResponse(
                        "User activated successfully")
            );
        }
    }
}