namespace StockFlow.Application.DTOs.Dashboard
{
    public class DashboardDto
    {
        // Catalog
        public int TotalProducts { get; set; }
        public int LowStockProducts { get; set; }
        public decimal InventoryValue { get; set; }

        // Inventory
        public int StockEntriesToday { get; set; }
        public int StockExitsToday { get; set; }
        public int StockAdjustmentsToday { get; set; }

        // Purchasing
        public int PendingInboundShipments { get; set; }
        public int OpenSupplierClaims { get; set; }

        // Sales
        public int TotalCustomers { get; set; }
        public int OrdersThisMonth { get; set; }
        public decimal SalesThisMonth { get; set; }

        // Logistics
        public int PendingDeliveries { get; set; }
        public int DeliveriesInTransit { get; set; }

        // Quality
        public int OpenQualityIssues { get; set; }
        public int OpenDeliveryIssues { get; set; }

        // Identity
        public int ActiveUsers { get; set; }
    }
}