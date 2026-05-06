using StockFlow.API.Application.DTOs.Reports;
using StockFlow.API.DTOs.Reports;

namespace StockFlow.API.Application.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<CurrentStockReportDto>> GetCurrentStockReportAsync();
        Task<IEnumerable<LowStockReportDto>> GetLowStockReportAsync();
        Task<IEnumerable<StockMovementReportDto>> GetStockMovementReportAsync(StockMovementReportQueryParameters queryParameters);

        Task<byte[]> ExportCurrentStockCsvAsync();
        Task<byte[]> ExportCurrentStockExcelAsync();
        Task<byte[]> ExportStockMovementsCsvAsync(StockMovementReportQueryParameters query);
        Task<byte[]> ExportStockMovementsExcelAsync(StockMovementReportQueryParameters query);
    }
}
