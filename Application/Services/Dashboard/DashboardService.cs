using StockFlow.Application.DTOs.Dashboard;
using StockFlow.Application.Interfaces.Catalog;
using StockFlow.Application.Interfaces.Dashboard;
using StockFlow.Application.Interfaces.Identity;
using StockFlow.Application.Interfaces.Inventory;
using StockFlow.Application.Interfaces.Logistics;
using StockFlow.Application.Interfaces.Purchasing;
using StockFlow.Application.Interfaces.Quality;
using StockFlow.Application.Interfaces.Sales;

namespace StockFlow.Application.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IInboundShipmentRepository _inboundShipmentRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IDeliveryIssueRepository _deliveryIssueRepository;
        private readonly IQualityIssueRepository _qualityIssueRepository;
        private readonly IUserRepository _userRepository;

        public DashboardService(
            IProductRepository productRepository,
            IStockMovementRepository stockMovementRepository,
            IInboundShipmentRepository inboundShipmentRepository,
            ISalesOrderRepository salesOrderRepository,
            ICustomerRepository customerRepository,
            IDeliveryRepository deliveryRepository,
            IDeliveryIssueRepository deliveryIssueRepository,
            IQualityIssueRepository qualityIssueRepository,
            IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _stockMovementRepository = stockMovementRepository;
            _inboundShipmentRepository = inboundShipmentRepository;
            _salesOrderRepository = salesOrderRepository;
            _customerRepository = customerRepository;
            _deliveryRepository = deliveryRepository;
            _deliveryIssueRepository = deliveryIssueRepository;
            _qualityIssueRepository = qualityIssueRepository;
            _userRepository = userRepository;
        }

        public async Task<DashboardDto> GetDashboardAsync()
        {
            return new DashboardDto
            {
                // Catalog
                TotalProducts = 0,
                LowStockProducts = 0,
                InventoryValue = 0,

                // Inventory
                StockEntriesToday = 0,
                StockExitsToday = 0,
                StockAdjustmentsToday = 0,

                // Purchasing
                PendingInboundShipments = 0,
                OpenSupplierClaims = 0,

                // Sales
                TotalCustomers = 0,
                OrdersThisMonth = 0,
                SalesThisMonth = 0,

                // Logistics
                PendingDeliveries = 0,
                DeliveriesInTransit = 0,

                // Quality
                OpenQualityIssues = 0,
                OpenDeliveryIssues = 0,

                // Identity
                ActiveUsers = 0
            };
        }
    }
}