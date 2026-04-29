using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StockFlow.API.DTOs.Dashboard;
using StockFlow.API.Interfaces;
using StockFlow.API.Models;

namespace StockFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Dashboard")]
    [Authorize(Policy = "DashboardAccess")] 
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockProducts()
        {
            var products = await _dashboardService.GetLowStockProductsAsync();

            return Ok(new ApiResponse<IEnumerable<LowStockProductDto>>
            {
                Success = true,
                Message = "Low stock products retrieved successfully.",
                Data = products
            });
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _dashboardService.GetSummaryAsync();

            return Ok(new ApiResponse<DashboardSummaryDto>
            {
                Success = true,
                Message = "Dashboard summary retrieved successfully.",
                Data = summary
            });
        }

        [HttpGet("recent-movements")]
        public async Task<IActionResult> GetRecentMovements([FromQuery] int count = 10)
        {
            var movements = await _dashboardService.GetRecentMovementsAsync(count);

            return Ok(new ApiResponse<IEnumerable<RecentMovementDto>>
            {
                Success = true,
                Message = "Recent stock movements retrieved successfully.",
                Data = movements
            });
        }
    }
}