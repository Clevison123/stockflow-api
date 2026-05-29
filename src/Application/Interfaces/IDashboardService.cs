using StockFlow.API.src.Application.DTOs.Dashboard;

namespace StockFlow.API.src.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<IEnumerable<LowStockProductDto>> GetLowStockProductsAsync();
        Task<DashboardSummaryDto> GetSummaryAsync();
        Task<IEnumerable<RecentMovementDto>> GetRecentMovementsAsync(int count = 10);
    }
} 