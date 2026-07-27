using StockFlow.Application.DTOs.Reports;
using StockFlow.Application.DTOs.Reports.Audit;
using StockFlow.Application.DTOs.Reports.Catalog;
using StockFlow.Application.DTOs.Reports.Delivery;
using StockFlow.Application.DTOs.Reports.Inventory;
using StockFlow.Application.DTOs.Reports.Quality;
using StockFlow.Application.DTOs.Reports.Sales;
using StockFlow.Application.DTOs.Reports.Shipment;

namespace StockFlow.Application.Interfaces.Reports
{
    public interface IReportService
    {
        // Catalog / Inventory
        Task<IEnumerable<CurrentStockReportDto>> GetCurrentStockReportAsync();

        Task<IEnumerable<LowStockReportDto>> GetLowStockReportAsync();

        Task<IEnumerable<InventoryValueReportDto>> GetInventoryValueReportAsync();

        Task<IEnumerable<StockMovementReportDto>> GetStockMovementReportAsync();


        // Sales
        Task<IEnumerable<CustomerSalesReportDto>> GetCustomerSalesReportAsync();

        Task<IEnumerable<SalesOrderReportDto>> GetSalesOrderReportAsync();


        // Logistics
        Task<IEnumerable<DeliveryReportDto>> GetDeliveryReportAsync();

        Task<IEnumerable<DeliveryIssueReportDto>> GetDeliveryIssueReportAsync();


        // Purchasing
        Task<IEnumerable<InboundShipmentReportDto>> GetInboundShipmentReportAsync();


        // Quality
        Task<IEnumerable<QualityIssueReportDto>> GetQualityIssueReportAsync();


        // Audit
        Task<IEnumerable<AuditLogReportDto>> GetAuditLogReportAsync();
    }
}