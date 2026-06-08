namespace StockFlow.Application.DTOs.Dashboard
{
    public class DashboardDto
    {
        // Catalog
        public int TotalProducts { get; set; }

        public int LowStockProducts { get; set; }

        // Purchasing
        public int PendingInboundShipments { get; set; }

        // Sales
        public int TotalCustomers { get; set; }

        public int OrdersThisMonth { get; set; }

        // Logistics
        public int PendingDeliveries { get; set; }

        // Quality
        public int OpenQualityIssues { get; set; }

        // Identity
        public int ActiveUsers { get; set; }

        // Inventory
        public int StockMovementsToday { get; set; }
    }
}