using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.API.Presentation.Responses;
using StockFlow.API.src.Application.DTOs.Reports;
using StockFlow.API.src.Application.Interfaces;

namespace StockFlow.API.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Reports")]
    [Authorize(Policy = "ReportsAccess")] 
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("current-stock")]
        public async Task<IActionResult> GetCurrentStockReport()
        {
            var report = await _reportService.GetCurrentStockReportAsync();

            return Ok(new ApiResponse<IEnumerable<CurrentStockReportDto>>
            {
                Success = true,
                Message = "Current stock report retrieved successfully.",
                Data = report
            });
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockReport()
        {
            var report = await _reportService.GetLowStockReportAsync();

            return Ok(new ApiResponse<IEnumerable<LowStockReportDto>>
            {
                Success = true,
                Message = "Low stock report retrieved successfully.",
                Data = report
            });
        }

        [HttpGet("stock-movements")]
        public async Task<IActionResult> GetStockMovementReport([FromQuery] StockMovementReportQueryParameters queryParameters)
        {
            var report = await _reportService.GetStockMovementReportAsync(queryParameters);

            return Ok(new ApiResponse<IEnumerable<StockMovementReportDto>>
            {
                Success = true,
                Message = "Stock movement report retrieved successfully.",
                Data = report
            });
        }

        [HttpGet("export/current-stock/csv")]
        public async Task<IActionResult> ExportCurrentStockCsv()
        {
            var file = await _reportService.ExportCurrentStockCsvAsync();

            return File(file, "text/csv", "current-stock.csv");
        }

        [HttpGet("export/current-stock/excel")]
        public async Task<IActionResult> ExportCurrentStockExcel()
        {
            var file = await _reportService.ExportCurrentStockExcelAsync();

            return File(file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "current-stock.xlsx");
        }

        [HttpGet("export/stock-movements/csv")]
        public async Task<IActionResult> ExportStockMovementsCsv([FromQuery] StockMovementReportQueryParameters query)
        {
            var file = await _reportService.ExportStockMovementsCsvAsync(query);

            return File(file, "text/csv", "stock-movements.csv");
        }

        [HttpGet("export/stock-movements/excel")]
        public async Task<IActionResult> ExportStockMovementsExcel([FromQuery] StockMovementReportQueryParameters query)
        {
            var file = await _reportService.ExportStockMovementsExcelAsync(query);

            return File(file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "stock-movements.xlsx");
        }
    }
}