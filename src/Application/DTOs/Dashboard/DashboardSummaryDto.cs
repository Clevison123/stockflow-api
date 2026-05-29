namespace StockFlow.API.src.Application.DTOs.Dashboard
{
    public class DashboardSummaryDto
    {
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalSuppliers { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public int TotalStockMovements { get; set; }
        public int RecentEntries { get; set; }
        public int RecentExits { get; set; }

    }
}
