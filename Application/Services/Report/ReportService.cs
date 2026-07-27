using StockFlow.Application.DTOs.Catalog.Product;
using StockFlow.Application.DTOs.Reports;
using StockFlow.Application.DTOs.Reports.Audit;
using StockFlow.Application.DTOs.Reports.Catalog;
using StockFlow.Application.DTOs.Reports.Delivery;
using StockFlow.Application.DTOs.Reports.Inventory;
using StockFlow.Application.DTOs.Reports.Quality;
using StockFlow.Application.DTOs.Reports.Sales;
using StockFlow.Application.DTOs.Reports.Shipment;
using StockFlow.Application.Exceptions;
using StockFlow.Application.Interfaces.Catalog;
using StockFlow.Application.Interfaces.IAudit;
using StockFlow.Application.Interfaces.Inventory;
using StockFlow.Application.Interfaces.Logistics;
using StockFlow.Application.Interfaces.Purchasing;
using StockFlow.Application.Interfaces.Quality;
using StockFlow.Application.Interfaces.Reports;
using StockFlow.Application.Interfaces.Sales;

namespace StockFlow.Application.Services.Report
{
    public class ReportService : IReportService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IDeliveryIssueRepository _deliveryIssueRepository;
        private readonly IInboundShipmentRepository _inboundShipmentRepository;
        private readonly IQualityIssueRepository _qualityIssueRepository;
        private readonly IStockMovementRepository _stockMovementRepository;

        public ReportService(
            IAuditLogRepository auditLogRepository,
            IProductRepository productRepository,
            ISalesOrderRepository salesOrderRepository,
            IDeliveryRepository deliveryRepository,
            IDeliveryIssueRepository deliveryIssueRepository,
            IInboundShipmentRepository inboundShipmentRepository,
            IQualityIssueRepository qualityIssueRepository,
            IStockMovementRepository stockMovementRepository)
        {
            _auditLogRepository = auditLogRepository;
            _productRepository = productRepository;
            _salesOrderRepository = salesOrderRepository;
            _deliveryRepository = deliveryRepository;
            _deliveryIssueRepository = deliveryIssueRepository;
            _inboundShipmentRepository = inboundShipmentRepository;
            _qualityIssueRepository = qualityIssueRepository;
            _stockMovementRepository = stockMovementRepository;
        }

        public async Task<IEnumerable<AuditLogReportDto>> GetAuditLogReportAsync()
        {
            var logs = await _auditLogRepository.GetAllAsync();

            if (!logs.Any())
            {
                throw new NotFoundException("No audit logs were found.");
            }

            return logs.Select(log => new AuditLogReportDto
            {
                Timestamp = log.CreatedAt,
                UserEmail = log.UserEmail,
                Action = log.Action.ToString(),
                EntityName = log.Entity.ToString(),
                EntityId = log.EntityId,
                Success = log.Success,
                ErrorMessage = log.ErrorMessage
            });
        }

        public async Task<IEnumerable<CurrentStockReportDto>> GetCurrentStockReportAsync()
        {
            var queryParameters = new ProductQueryParametersDto
            {
                PageNumber = 1,
                PageSize = int.MaxValue
            };

            var result = await _productRepository.GetAllAsync(queryParameters);

            var products = result.Items;

            if (!products.Any())
            {
                throw new NotFoundException("No products were found.");
            }

            return products.Select(product => new CurrentStockReportDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                CategoryName = product.Category?.Name ?? string.Empty,
                QuantityInStock = product.QuantityInStock,
                MinimumStock = product.MinimumStock
            });
        }

        public async Task<IEnumerable<CustomerSalesReportDto>> GetCustomerSalesReportAsync()
        {
            var orders = await _salesOrderRepository.GetAllAsync();

            if (!orders.Any())
            {
                throw new NotFoundException("No sales orders were found.");
            }

            return orders
                .GroupBy(order => order.Customer)
                .Select(group => new CustomerSalesReportDto
                {
                    CustomerName = group.Key.TradeName,
                    TotalOrders = group.Count(),
                    TotalPurchased = group.Sum(x => x.TotalAmount),
                    IsPriorityCustomer = group.Sum(x => x.TotalAmount) >= 10000
                });
        }

        public async Task<IEnumerable<DeliveryIssueReportDto>> GetDeliveryIssueReportAsync()
        {
            var issues = await _deliveryIssueRepository.GetAllAsync();

            if (!issues.Any())
            {
                throw new NotFoundException("No delivery issues were found.");
            }

            return issues.Select(issue => new DeliveryIssueReportDto
            {
                DeliveryId = issue.DeliveryId,
                IssueType = issue.IssueType.ToString(),
                Description = issue.Description,
                IsResolved = issue.IsResolved,
                OccurredAt = issue.OccurredAt,
                ResolvedAt = issue.ResolvedAt
            });
        }

        public async Task<IEnumerable<DeliveryReportDto>> GetDeliveryReportAsync()
        {
            var deliveries = await _deliveryRepository.GetAllAsync();

            if (!deliveries.Any())
            {
                throw new NotFoundException("No deliveries were found.");
            }

            return deliveries.Select(delivery => new DeliveryReportDto
            {
                DeliveryId = delivery.Id,
                CustomerName = delivery.SalesOrder.Customer.TradeName,
                DeliveryDate = delivery.DeliveredAt ?? delivery.DepartureDate,
                Status = delivery.Status.ToString()
            });
        }

        public async Task<IEnumerable<InboundShipmentReportDto>> GetInboundShipmentReportAsync()
        {
            var shipments = await _inboundShipmentRepository.GetAllAsync();

            if (!shipments.Any())
            {
                throw new NotFoundException("No inbound shipments were found.");
            }

            return shipments.Select(shipment => new InboundShipmentReportDto
            {
                ShipmentNumber = shipment.ShipmentNumber,
                SupplierName = shipment.Supplier?.Name ?? string.Empty,
                ArrivalDate = shipment.ArrivalDate,
                Status = shipment.Status.ToString()
            });
        }

        public async Task<IEnumerable<InventoryValueReportDto>> GetInventoryValueReportAsync()
        {
            var products = await _productRepository.GetAllAsync(new ProductQueryParametersDto());

            if (!products.Items.Any())
            {
                throw new NotFoundException("No products were found.");
            }

            return products.Items.Select(product => new InventoryValueReportDto
            {
                ProductName = product.Name,
                QuantityInStock = product.QuantityInStock,
                UnitCost = product.PurchasePrice,
                TotalValue = product.QuantityInStock * product.PurchasePrice
            });
        }

        public async Task<IEnumerable<LowStockReportDto>> GetLowStockReportAsync()
        {
            var products = await _productRepository.GetLowStockAsync();

            if (!products.Any())
            {
                throw new NotFoundException("No low stock products were found.");
            }

            return products.Select(product => new LowStockReportDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                CurrentStock = product.QuantityInStock,
                MinimumStock = product.MinimumStock
            });
        }

        public async Task<IEnumerable<QualityIssueReportDto>> GetQualityIssueReportAsync()
        {
            var issues = await _qualityIssueRepository.GetAllAsync();

            if (!issues.Any())
            {
                throw new NotFoundException("No quality issues were found.");
            }

            return issues.Select(issue => new QualityIssueReportDto
            {
                ProductName = issue.ProductItem.Product.Name,
                IssueType = issue.IssueType.ToString(),
                Description = issue.Description,
                RequiresSupplierClaim = issue.RequiresSupplierClaim,
                IsResolved = issue.IsResolved,
                DetectedAt = issue.DetectedAt
            });
        }

        public async Task<IEnumerable<SalesOrderReportDto>> GetSalesOrderReportAsync()
        {
            var orders = await _salesOrderRepository.GetAllAsync();

            if (!orders.Any())
            {
                throw new NotFoundException("No sales orders were found.");
            }

            return orders.Select(order => new SalesOrderReportDto
            {
                OrderNumber = order.OrderNumber,
                CustomerName = order.Customer.TradeName,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount
            });
        }

        public async Task<IEnumerable<StockMovementReportDto>> GetStockMovementReportAsync()
        {
            var movements = await _stockMovementRepository.GetAllAsync();

            if (!movements.Any())
            {
                throw new NotFoundException("No stock movements were found.");
            }

            return movements.Select(movement => new StockMovementReportDto
            {
                ProductName = movement.Product.Name,
                MovementType = movement.MovementType,
                Quantity = movement.Quantity,
                Reason = movement.Reason,
                Date = movement.CreatedAt
            });
        }
    }
}
