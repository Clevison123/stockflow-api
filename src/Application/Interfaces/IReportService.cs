using StockFlow.API.src.Application.DTOs.Reports;

namespace StockFlow.API.src.Application.Interfaces
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
