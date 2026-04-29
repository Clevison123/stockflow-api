using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.API.Constants;
using StockFlow.API.DTOs.Product;
using StockFlow.API.Extensions;
using StockFlow.API.Helpers;
using StockFlow.API.Interfaces;
using StockFlow.API.Models;

namespace StockFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Products")]
    [Authorize] 
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICurrentUserService _currentUserService;

        public ProductsController(IProductService productService, ICurrentUserService currentUserService)
        {
            _productService = productService;
            _currentUserService = currentUserService;
        }

        //Público
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] ProductQueryParameters queryParameters)
        {
            var products = await _productService.GetAllProductsAsync(queryParameters);

            var response = ApiResponse<object>.SuccessResponse(products, "Products retrieved successfully.");

            return Ok(response);
        }

        //Público
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdOrThrowAsync(id);

            var response = ApiResponse<object>.SuccessResponse(product, "Product retrieved successfully.");

            return Ok(response);
        }

        //Apenas Admin
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productService.CreateProductAsync(dto);

            var response = ApiResponse<object>.SuccessResponse(product, "Product created successfully.");

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, response);
        }

        //Apenas Admin
        [HttpPut("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedProduct = await _productService.UpdateProductAsync(id, dto);

            var response = ApiResponse<object>.SuccessResponse(updatedProduct, "Product updated successfully.");

            return Ok(response);
        }

        //Apenas Admin
        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);

            return NoContent();
        }

        //Usuário autenticado
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var data = new
            {
                UserId = _currentUserService.UserId,
                FullName = _currentUserService.FullName,
                Email = _currentUserService.Email,
                Role = _currentUserService.Role,
                IsAuthenticated = _currentUserService.IsAuthenticated
            };

            var response = ApiResponse<object>.SuccessResponse(data, "Authenticated user data retrieved.");

            return Ok(response);
        }

        //Usuário autenticado 
        [HttpGet("me-extension")]
        public IActionResult ExtensionTest()
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