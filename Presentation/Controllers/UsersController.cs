using Microsoft.AspNetCore.Mvc;
using StockFlow.API.Application.DTOs.Users;
using StockFlow.API.Application.Interfaces;

namespace StockFlow.API.Presentation.Controllers
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
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();

            return Ok(users);
        }

        // GET: api/users/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateUserDto dto)
        {
            var createdUser =
                await _userService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdUser.Id },
                createdUser);
        }

        // PUT: api/users/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateUserDto dto)
        {
            var updatedUser =
                await _userService.UpdateAsync(id, dto);

            return Ok(updatedUser);
        }

        // PATCH: api/users/1/deactivate
        [HttpPatch("{id:int}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            await _userService.DeactivateAsync(id);

            return NoContent();
        }

        // PATCH: api/users/1/activate
        [HttpPatch("{id:int}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            await _userService.ActivateAsync(id);

            return NoContent();
        }
    }
}