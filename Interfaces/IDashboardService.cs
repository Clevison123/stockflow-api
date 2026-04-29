using StockFlow.API.DTOs.Dashboard;

namespace StockFlow.API.Interfaces
{
    public interface IDashboardService
    {
        Task<IEnumerable<LowStockProductDto>> GetLowStockProductsAsync();
        Task<DashboardSummaryDto> GetSummaryAsync();
        Task<IEnumerable<RecentMovementDto>> GetRecentMovementsAsync(int count = 10);
    }
} 