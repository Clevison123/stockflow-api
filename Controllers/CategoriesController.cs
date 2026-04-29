using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.API.DTOs.Category;
using StockFlow.API.Models;
using StockFlow.API.Services;
using StockFlow.API.Extensions;

namespace StockFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Categories")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //Criar categoria (pode restringir depois para Admin)
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _categoryService.CreateCategoryAsync(dto);

            var response = ApiResponse<object>.SuccessResponse(category, "Category created successfully.");

            return StatusCode(StatusCodes.Status201Created, response);
        }

        //Público (se quiser liberar leitura)
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var categories = _categoryService.GetAllCategories();

            var response = ApiResponse<object>.SuccessResponse(categories, "Categories retrieved successfully.");

            return Ok(response);
        }

        //Público
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var category = _categoryService.GetCategoryById(id);

            if (category == null)
            {
                var errorResponse = ApiResponse<object>.ErrorResponse("Category not found.");
                return NotFound(errorResponse);
            }

            var response = ApiResponse<object>.SuccessResponse(category, "Category retrieved successfully.");

            return Ok(response);
        }

        //Atualizar
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, dto);

            if (updatedCategory == null)
            {
                var errorResponse = ApiResponse<object>.ErrorResponse("Category not found.");
                return NotFound(errorResponse);
            }

            var response = ApiResponse<object>.SuccessResponse(updatedCategory, "Category updated successfully.");

            return Ok(response);
        }

        //Deletar
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoryService.DeleteCategoryAsync(id);

            if (!deleted)
            {
                var errorResponse = ApiResponse<object>.ErrorResponse("Category not found.");
                return NotFound(errorResponse);
            }

            return NoContent(); //padrão REST
        }

        //Teste de usuário
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