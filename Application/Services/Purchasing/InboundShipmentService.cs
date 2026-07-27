using FluentValidation;
using StockFlow.Application.DTOs.Audit;
using StockFlow.Application.DTOs.Purchasing.InboundShipment;
using StockFlow.Application.DTOs.Purchasing.InboundShipmentItem;
using StockFlow.Application.Exceptions;
using StockFlow.Application.Interfaces.Catalog;
using StockFlow.Application.Interfaces.IAudit;
using StockFlow.Application.Interfaces.Identity;
using StockFlow.Application.Interfaces.Purchasing;
using StockFlow.Application.Interfaces.Purchasing.IServices;
using StockFlow.Domain.Entities.Purchasing;
using StockFlow.Domain.Enums;
using StockFlow.Domain.Enums.Audit;
using System.Text.Json;

namespace StockFlow.Application.Services.Purchasing
{
    public class InboundShipmentService : IInboundShipmentService
    {
        private readonly IInboundShipmentRepository _inboundShipmentRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuditService _auditService;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductRepository _productRepository;
        private readonly IValidator<CreateInboundShipmentDto> _createValidator;
        private readonly IValidator<UpdateInboundShipmentDto> _updateValidator;
        
        public InboundShipmentService(
            IInboundShipmentRepository inboundShipmentRepository,
            ICurrentUserService currentUserService,
            IAuditService auditService,
            ISupplierRepository supplierRepository,
            IProductRepository productRepository,
            IValidator<CreateInboundShipmentDto> createValidator,
            IValidator<UpdateInboundShipmentDto> updateValidator)
        {
            _inboundShipmentRepository = inboundShipmentRepository;
            _currentUserService = currentUserService;
            _auditService = auditService;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        public async Task<InboundShipmentResponseDto> CreateAsync(CreateInboundShipmentDto dto)
        {
            var createValidation = await _createValidator.ValidateAsync(dto);

            if (!createValidation.IsValid)
            {
                throw new ApplicationValidationException( createValidation.Errors.Select(x => x.ErrorMessage) .ToList());
            }

            var supplier = await _supplierRepository.GetByIdAsync(dto.SupplierId);

            if (supplier is null)
            {
                throw new NotFoundException($"Supplier with ID {dto.SupplierId} was not found.");
            }

            var shipmentExists = await _inboundShipmentRepository .GetByShipmentNumberAsync(dto.ShipmentNumber);

            if (shipmentExists is not null)
            {
                throw new BusinessRuleException("An inbound shipment with this shipment number already exists.");
            }

            foreach (var item in dto.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);

                if (product is null)
                {
                    throw new NotFoundException(
                        $"Product with ID {item.ProductId} was not found.");
                }
            }

            var inboundShipment = new InboundShipment
            {
                ShipmentNumber = dto.ShipmentNumber,

                ContainerNumber = dto.ContainerNumber,

                OriginCountry = dto.OriginCountry,

                ArrivalDate = dto.ArrivalDate,

                SupplierId = dto.SupplierId,

                Notes = dto.Notes,

                Status = InboundShipmentStatus.Created,

                Items = dto.Items.Select(item => new InboundShipmentItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };

            await _inboundShipmentRepository.AddAsync(inboundShipment);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Create,

                Entity = AuditEntity.InboundShipment,

                EntityId = inboundShipment.Id.ToString(),

                NewValues = JsonSerializer.Serialize(inboundShipment),

                Success = true
            });

            return await GetByIdAsync(inboundShipment.Id);
        }

        public async Task<IEnumerable<InboundShipmentResponseDto>> GetAllAsync()
        {
            var shipments = await _inboundShipmentRepository.GetAllAsync();

            return shipments.Select(shipment => new InboundShipmentResponseDto
            {
                Id = shipment.Id,
                ShipmentNumber = shipment.ShipmentNumber,
                ContainerNumber = shipment.ContainerNumber,
                OriginCountry = shipment.OriginCountry,
                ArrivalDate = shipment.ArrivalDate,
                Status = shipment.Status,
                SupplierName = shipment.Supplier.Name,
                Notes = shipment.Notes,

                Items = shipment.Items.Select(item => new InboundShipmentItemResponseDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity
                }).ToList()
            });
        }

        public async Task<InboundShipmentResponseDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid shipment id.");
            }

            var shipment = await _inboundShipmentRepository.GetWithItemsAsync(id);

            if (shipment is null)
            {
                throw new NotFoundException($"Shipment with ID {id} was not found.");
            }

            return new InboundShipmentResponseDto
            {
                Id = shipment.Id,
                ShipmentNumber = shipment.ShipmentNumber,
                ContainerNumber = shipment.ContainerNumber,
                OriginCountry = shipment.OriginCountry,
                ArrivalDate = shipment.ArrivalDate,
                Status = shipment.Status,
                SupplierName = shipment.Supplier.Name,
                Notes = shipment.Notes,

                Items = shipment.Items.Select(item => new InboundShipmentItemResponseDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity
                }).ToList()
            };
        }

        public async Task<IEnumerable<InboundShipmentResponseDto>> GetByStatusAsync(InboundShipmentStatus status)
        {
            if (!Enum.IsDefined(typeof(InboundShipmentStatus), status))
            {
                throw new BadRequestException("Invalid inbound shipment status.");
            }

            var shipments = await _inboundShipmentRepository.GetByStatusAsync(status);

            if (!shipments.Any())
            {
                throw new NotFoundException(
                    $"No inbound shipments found with status {status}.");
            }

            return shipments.Select(shipment => new InboundShipmentResponseDto
            {
                Id = shipment.Id,
                ShipmentNumber = shipment.ShipmentNumber,
                ContainerNumber = shipment.ContainerNumber,
                OriginCountry = shipment.OriginCountry,
                ArrivalDate = shipment.ArrivalDate,
                Status = shipment.Status,
                SupplierName = shipment.Supplier.Name,
                Notes = shipment.Notes,

                Items = shipment.Items.Select(item => new InboundShipmentItemResponseDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity
                }).ToList()
            });
        }

        public async Task<IEnumerable<InboundShipmentResponseDto>> GetBySupplierAsync(int supplierId)
        {
            if (supplierId <= 0)
            {
                throw new BadRequestException("Invalid supplier id.");
            }

            var supplier = await _supplierRepository.GetByIdAsync(supplierId);

            if (supplier is null)
            {
                throw new NotFoundException($"Supplier with ID {supplierId} was not found.");
            }

            var shipments = await _inboundShipmentRepository.GetBySupplierAsync(supplierId);

            return shipments.Select(shipment => new InboundShipmentResponseDto
            {
                Id = shipment.Id,
                ShipmentNumber = shipment.ShipmentNumber,
                ContainerNumber = shipment.ContainerNumber,
                OriginCountry = shipment.OriginCountry,
                ArrivalDate = shipment.ArrivalDate,
                Status = shipment.Status,
                SupplierName = shipment.Supplier.Name,
                Notes = shipment.Notes,

                Items = shipment.Items.Select(item => new InboundShipmentItemResponseDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity
                }).ToList()
            });
        }

        public async Task<InboundShipmentResponseDto> UpdateAsync( int id,UpdateInboundShipmentDto dto)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid shipment id.");
            }

            var validationUpdate = await _updateValidator.ValidateAsync(dto);

            if (!validationUpdate.IsValid)
            {
                throw new ApplicationValidationException( validationUpdate.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var shipment = await _inboundShipmentRepository.GetByIdAsync(id);

            if (shipment is null)
            {
                throw new NotFoundException( $"Inbound shipment with ID {id} was not found.");
            }

            var oldValues = JsonSerializer.Serialize(shipment);

            shipment.ArrivalDate = dto.ArrivalDate;
            shipment.Notes = dto.Notes;

            await _inboundShipmentRepository.UpdateAsync(shipment);

            var newValues = JsonSerializer.Serialize(shipment);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.InboundShipment,
                EntityId = shipment.Id.ToString(),
                OldValues = oldValues,
                NewValues = newValues,
                Success = true
            });

            return await GetByIdAsync(shipment.Id);
        }

        public async Task UpdateStatusAsync(int shipmentId, UpdateInboundShipmentStatusDto dto)
        {
            if (shipmentId <= 0)
            {
                throw new BadRequestException("Invalid shipment id.");
            }

            if (!Enum.IsDefined(typeof(InboundShipmentStatus), dto.Status))
            {
                throw new BadRequestException("Invalid shipment status.");
            }

            var shipment = await _inboundShipmentRepository.GetByIdAsync(shipmentId);

            if (shipment is null)
            {
                throw new NotFoundException($"Inbound shipment with ID {shipmentId} was not found.");
            }

            if (shipment.Status == dto.Status)
            {
                throw new ConflictException($"Shipment is already in status {dto.Status}.");
            }

            if (shipment.Status == InboundShipmentStatus.Completed)
            {
                throw new ConflictException("A completed shipment cannot change status.");
            }


            if (shipment.Status == InboundShipmentStatus.Rejected)
            {
                throw new ConflictException("A rejected shipment cannot change status.");
            }

            var oldValues = JsonSerializer.Serialize(shipment);

            shipment.Status = dto.Status;

            await _inboundShipmentRepository.UpdateAsync(shipment);

            var newValues = JsonSerializer.Serialize(shipment);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.InboundShipment,
                EntityId = shipment.Id.ToString(),
                OldValues = oldValues,
                NewValues = newValues,
                Success = true
            });
        }
        public async Task CancelAsync(int shipmentId, string reason)
        {
            if (shipmentId <= 0)
            {
                throw new BadRequestException("Invalid shipment id.");
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new BadRequestException("Cancellation reason is required.");
            }

            var shipment = await _inboundShipmentRepository.GetByIdAsync(shipmentId);

            if (shipment is null)
            {
                throw new NotFoundException($"Inbound shipment with ID {shipmentId} was not found.");
            }

            if (shipment.Status == InboundShipmentStatus.Completed)
            {
                throw new ConflictException("A completed shipment cannot be cancelled.");
            }

            if (shipment.Status == InboundShipmentStatus.Rejected)
            {
                throw new ConflictException("A rejected shipment is already cancelled.");
            }

            var oldValues = JsonSerializer.Serialize(shipment);

            shipment.Status = InboundShipmentStatus.Rejected;

            shipment.Notes += $" | Cancelled: {reason}";

            await _inboundShipmentRepository.UpdateAsync(shipment);
             
            var newValues = JsonSerializer.Serialize(shipment);
             
            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,

                Entity = AuditEntity.InboundShipment,

                EntityId = shipment.Id.ToString(),

                OldValues = oldValues,

                NewValues = newValues,

                Success = true
            });
        }

        public async Task ConfirmReceivedAsync(int shipmentId)
        { 
            if (shipmentId <= 0)
            {
                throw new BadRequestException("Invalid shipment id.");
            }
             
            var shipment = await _inboundShipmentRepository.GetByIdAsync(shipmentId);
             
            if (shipment is null)
            {
                throw new NotFoundException($"Inbound shipment with ID {shipmentId} was not found.");
            }
             
            if (shipment.Status == InboundShipmentStatus.Rejected)
            {
                throw new ConflictException("A rejected shipment cannot be received.");
            }
             
            if (shipment.Status == InboundShipmentStatus.Received ||
                shipment.Status == InboundShipmentStatus.Completed)
            {
                throw new ConflictException("This shipment has already been received.");
            }
             
            var oldValues = JsonSerializer.Serialize(shipment);
             
            shipment.Status = InboundShipmentStatus.Received;
             
            await _inboundShipmentRepository.UpdateAsync(shipment);
             
            var newValues = JsonSerializer.Serialize(shipment);
             
            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.InboundShipment,
                EntityId = shipment.Id.ToString(),
                OldValues = oldValues,
                NewValues = newValues,
                Success = true
            });
        }
    }
}
