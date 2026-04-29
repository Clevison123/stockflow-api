using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.API.DTOs.StockMovement;
using StockFlow.API.Interfaces;
using StockFlow.API.Models;

namespace StockFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "StockMovements")]
    [Authorize] 
    public class StockMovementsController : ControllerBase
    {
        private readonly IStockMovementService _stockMovementService;

        public StockMovementsController(IStockMovementService stockMovementService)
        {
            _stockMovementService = stockMovementService;
        }

        //Entrada no estoque
        [HttpPost("in")]
        [Authorize(Policy = "StockMovementWrite")]
        public async Task<IActionResult> RegisterEntry([FromBody] CreateStockMovementDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movement = await _stockMovementService.RegisterEntryAsync(dto);

            return StatusCode(StatusCodes.Status201Created, new ApiResponse<object>
            {
                Success = true,
                Message = "Stock entry registered successfully.",
                Data = movement
            });
        }

        //Saída do estoque
        [HttpPost("out")]
        [Authorize(Policy = "StockMovementWrite")]
        public async Task<IActionResult> RegisterExit([FromBody] CreateStockMovementDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movement = await _stockMovementService.RegisterExitAsync(dto);

            return StatusCode(StatusCodes.Status201Created, new ApiResponse<object>
            {
                Success = true,
                Message = "Stock exit registered successfully.",
                Data = movement
            });
        }

        //Visualizar tudo
        [HttpGet]
        [Authorize(Policy = "StockMovementRead")]
        public async Task<IActionResult> GetAll()
        {
            var movements = await _stockMovementService.GetAllAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Stock movements retrieved successfully.",
                Data = movements
            });
        }

        //Visualizar por produto
        [HttpGet("product/{productId:int}")]
        [Authorize(Policy = "StockMovementRead")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var movements = await _stockMovementService.GetByProductIdAsync(productId);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Product stock movements retrieved successfully.",
                Data = movements
            });
        }
    }
}