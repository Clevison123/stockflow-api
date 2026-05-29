using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.API.src.Application.DTOs.Supplier;
using StockFlow.API.src.Application.Services;
using StockFlow.API.src.Infrastructure.Extensions;
using StockFlow.API.src.Presentation.Responses;

namespace StockFlow.API.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Suppliers")]
    [Authorize] 
    public class SuppliersController : ControllerBase
    {
        private readonly SupplierService _supplierService;

        public SuppliersController(SupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        //Criar fornecedor (Admin / Manager)
        [HttpPost]
        [Authorize(Policy = "SupplierWrite")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateSupplierDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var supplier = await _supplierService.CreateSupplierAsync(dto);

            var response = ApiResponse<object>.SuccessResponse(supplier, "Supplier created successfully.");

            return StatusCode(StatusCodes.Status201Created, response);
        }

        //Visualizar todos (usuário logado)
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var suppliers = _supplierService.GetAllSuppliers();

            var response = ApiResponse<object>.SuccessResponse(suppliers, "Suppliers retrieved successfully.");

            return Ok(response);
        }

        //Visualizar por ID
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var supplier = _supplierService.GetSupplierById(id);

            if (supplier == null)
            {
                var error = ApiResponse<object>.ErrorResponse("Supplier not found.");
                return NotFound(error);
            }

            var response = ApiResponse<object>.SuccessResponse(supplier, "Supplier retrieved successfully.");
            return Ok(response);
        }

        //Atualizar (Admin / Manager)
        [HttpPut("{id:int}")]
        [Authorize(Policy = "SupplierWrite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSupplierDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedSupplier = await _supplierService.UpdateSupplierAsync(id, dto);

            if (updatedSupplier == null)
            {
                var error = ApiResponse<object>.ErrorResponse("Supplier not found.");
                return NotFound(error);
            }

            var response = ApiResponse<object>.SuccessResponse(updatedSupplier, "Supplier updated successfully.");
            return Ok(response);
        }

        //Deletar (somente Admin)
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "SupplierDelete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _supplierService.DeleteSupplierAsync(id);

            if (!deleted)
            {
                var error = ApiResponse<object>.ErrorResponse("Supplier not found.");
                return NotFound(error);
            }

            return NoContent();
        }

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