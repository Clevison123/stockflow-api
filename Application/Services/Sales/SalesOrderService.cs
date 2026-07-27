using FluentValidation;
using StockFlow.Application.DTOs.Audit;
using StockFlow.Application.DTOs.Sales.SalesOrder;
using StockFlow.Application.DTOs.Sales.SalesOrderItem;
using StockFlow.Application.Exceptions;
using StockFlow.Application.Interfaces.Catalog;
using StockFlow.Application.Interfaces.IAudit;
using StockFlow.Application.Interfaces.Sales;
using StockFlow.Application.Interfaces.Sales.IServices;
using StockFlow.Domain.Entities.Sales;
using StockFlow.Domain.Enums.Audit;
using StockFlow.Domain.Enums.Sales;
using System.Text.Json;

namespace StockFlow.Application.Services.Sales
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAuditService _auditService;

        private readonly IValidator<CreateSalesOrderDto> _createValidator;
        private readonly IValidator<UpdateSalesOrderDto> _updateValidator;
        private readonly IValidator<UpdateSalesOrderStatusDto> _updateStatusValidator;

        public SalesOrderService(
            ISalesOrderRepository salesOrderRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IAuditService auditService,
            IValidator<CreateSalesOrderDto> createValidator,
            IValidator<UpdateSalesOrderDto> updateValidator,
            IValidator<UpdateSalesOrderStatusDto> updateStatusValidator)
        {
            _salesOrderRepository = salesOrderRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _auditService = auditService;

            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _updateStatusValidator = updateStatusValidator;
        }

        public async Task<SalesOrderResponseDto> CreateAsync(CreateSalesOrderDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);

            if (customer is null)
            {
                throw new NotFoundException($"Customer with ID {dto.CustomerId} was not found.");
            }

            var order = new SalesOrder
            {
                CustomerId = dto.CustomerId,
                OrderNumber = Guid.NewGuid().ToString("N")[..10].ToUpper(),
                OrderDate = DateTime.UtcNow,
                Status = SalesOrderStatus.Pending,
                Notes = dto.Notes
            };

            decimal totalAmount = 0;

            foreach (var item in dto.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);

                if (product is null)
                {
                    throw new NotFoundException($"Product with ID {item.ProductId} was not found.");
                }

                var orderItem = new SalesOrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.SalePrice,
                    TotalPrice = product.SalePrice * item.Quantity
                };

                totalAmount += orderItem.TotalPrice;

                order.Items.Add(orderItem);
            }

            order.TotalAmount = totalAmount;

            await _salesOrderRepository.AddAsync(order);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Create,
                Entity = AuditEntity.SalesOrder,
                EntityId = order.Id.ToString(),
                NewValues = JsonSerializer.Serialize(order),
                Success = true
            });

            return await GetByIdAsync(order.Id);
        }

        public async Task<IEnumerable<SalesOrderResponseDto>> GetAllAsync()
        {
            var orders = await _salesOrderRepository.GetAllAsync();

            if (!orders.Any())
            {
                throw new NotFoundException("No sales orders were found.");
            }

            return orders.Select(order => new SalesOrderResponseDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.TradeName,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Notes = order.Notes,
                Items = order.Items.Select(item => new SalesOrderItemResponseDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            });
        }

        public async Task<IEnumerable<SalesOrderResponseDto>> GetByCustomerAsync(int customerId)
        {
            if (customerId <= 0)
            {
                throw new BadRequestException("Invalid customer ID.");
            }

            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer is null)
            {
                throw new NotFoundException($"Customer with ID {customerId} was not found.");
            }

            var orders = await _salesOrderRepository.GetByCustomerIdAsync(customerId);

            if (!orders.Any())
            {
                throw new NotFoundException(
                    $"No sales orders found for customer ID {customerId}.");
            }

            return orders.Select(order => new SalesOrderResponseDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.TradeName,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Notes = order.Notes,
                Items = order.Items.Select(item => new SalesOrderItemResponseDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            });
        }

        public async Task<SalesOrderResponseDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid sales order ID.");
            }

            var order = await _salesOrderRepository.GetWithItemsAsync(id);

            if (order is null)
            {
                throw new NotFoundException($"Sales order with ID {id} was not found.");
            }

            return new SalesOrderResponseDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.TradeName,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Notes = order.Notes,
                Items = order.Items.Select(item => new SalesOrderItemResponseDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };
        }

        public async Task<IEnumerable<SalesOrderResponseDto>> GetByStatusAsync(SalesOrderStatus status)
        {
            var orders = await _salesOrderRepository.GetByStatusAsync(status);

            if (!orders.Any())
            {
                throw new NotFoundException(
                    $"No sales orders were found with status {status}.");
            }

            return orders.Select(order => new SalesOrderResponseDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CustomerName = order.Customer.TradeName,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Notes = order.Notes,
                Items = order.Items.Select(item => new SalesOrderItemResponseDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            });
        }

        public async Task<SalesOrderResponseDto> UpdateAsync(int id, UpdateSalesOrderDto dto)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid sales order ID.");
            }

            var validationResult = await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(
                    validationResult.Errors
                        .Select(x => x.ErrorMessage)
                        .ToList());
            }

            var order = await _salesOrderRepository.GetWithItemsAsync(id);

            if (order is null)
            {
                throw new NotFoundException($"Sales order with ID {id} was not found.");
            }

            order.Notes = dto.Notes;

            await _salesOrderRepository.UpdateAsync(order);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.SalesOrder,
                EntityId = order.Id.ToString(),
                NewValues = JsonSerializer.Serialize(order),
                Success = true
            });

            return new SalesOrderResponseDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.TradeName,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Notes = order.Notes,
                Items = order.Items.Select(item => new SalesOrderItemResponseDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };
        }

        public async Task UpdateStatusAsync(int orderId, UpdateSalesOrderStatusDto dto)
        {
            if (orderId <= 0)
            {
                throw new BadRequestException("Invalid sales order ID.");
            }

            var validationResult = await _updateStatusValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var order = await _salesOrderRepository.GetByIdAsync(orderId);

            if (order is null)
            {
                throw new NotFoundException($"Sales order with ID {orderId} was not found.");
            }

            if (order.Status == dto.Status)
            {
                throw new BadRequestException($"Sales order is already {dto.Status}.");
            }

            order.Status = dto.Status;

            await _salesOrderRepository.UpdateAsync(order);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.SalesOrder,
                EntityId = order.Id.ToString(),
                NewValues = JsonSerializer.Serialize(order),
                Success = true
            });
        }

        public async Task CancelAsync(int orderId, string reason)
        {
            if (orderId <= 0)
            {
                throw new BadRequestException("Invalid sales order ID.");
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new BadRequestException("Cancellation reason is required.");
            }

            var order = await _salesOrderRepository.GetByIdAsync(orderId);

            if (order is null)
            {
                throw new NotFoundException($"Sales order with ID {orderId} was not found.");
            }

            if (order.Status == SalesOrderStatus.Cancelled)
            {
                throw new BadRequestException("Sales order is already cancelled.");
            }

            order.Status = SalesOrderStatus.Cancelled;

            order.Notes = string.IsNullOrWhiteSpace(order.Notes)
                ? $"Cancellation reason: {reason}"
                : $"{order.Notes}{Environment.NewLine}Cancellation reason: {reason}";

            await _salesOrderRepository.UpdateAsync(order);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.SalesOrder,
                EntityId = order.Id.ToString(),
                NewValues = JsonSerializer.Serialize(order),
                Success = true
            });
        }
    }
}
