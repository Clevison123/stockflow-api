using StockFlow.Application.DTOs.Dashboard;

namespace StockFlow.Application.Interfaces.Dashboard
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardAsync();
    }
}