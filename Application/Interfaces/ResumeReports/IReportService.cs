using StockFlow.Application.DTOs.ResumeReports.Reports;

namespace StockFlow.Application.Interfaces.ResumeReports
{
    public interface IReportService
    {
        // Inventory
        Task<IEnumerable<CurrentStockReportDto>>
            GetCurrentStockReportAsync();

        Task<IEnumerable<LowStockReportDto>>
            GetLowStockReportAsync();

        Task<IEnumerable<StockMovementReportDto>>
            GetStockMovementReportAsync();

        // Sales
        Task<IEnumerable<CustomerSalesReportDto>>
            GetCustomerSalesReportAsync();

        // Logistics
        Task<IEnumerable<DeliveryReportDto>>
            GetDeliveryReportAsync();

        // Purchasing
        Task<IEnumerable<InboundShipmentReportDto>>
            GetInboundShipmentReportAsync();

        // Quality
        Task<IEnumerable<QualityIssueReportDto>>
            GetQualityIssueReportAsync();

        // Audit
        Task<IEnumerable<AuditLogReportDto>>
            GetAuditLogReportAsync();
    }
}