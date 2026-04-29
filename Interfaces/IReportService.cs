using StockFlow.API.DTOs.Reports;
using StockFlow.API.Helpers;

namespace StockFlow.API.Interfaces
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
