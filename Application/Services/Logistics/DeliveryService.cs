using FluentValidation;
using StockFlow.Application.DTOs.Audit;
using StockFlow.Application.DTOs.Logistics.Delivery;
using StockFlow.Application.Exceptions;
using StockFlow.Application.Interfaces.IAudit;
using StockFlow.Application.Interfaces.Logistics;
using StockFlow.Application.Interfaces.Sales;
using StockFlow.Domain.Entities.Logistics;
using StockFlow.Domain.Enums.Audit;
using StockFlow.Domain.Enums.Logistics;
using System.Text.Json;

namespace StockFlow.Application.Services.Logistics
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IValidator<CreateDeliveryDto> _createDeliveryValidator;
        private readonly IValidator<UpdateDeliveryDto> _updateDeliveryValidator;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IAuditService _auditService;
        public DeliveryService(
            IDeliveryRepository deliveryRepository,
            ISalesOrderRepository salesOrderRepository,
            IValidator<CreateDeliveryDto> createDeliveryValidator,
            IValidator<UpdateDeliveryDto> updateDeliveryValidator,
            IAuditService auditService)
        {
            _deliveryRepository = deliveryRepository;
            _salesOrderRepository = salesOrderRepository;
            _createDeliveryValidator = createDeliveryValidator;
            _updateDeliveryValidator = updateDeliveryValidator;
            _auditService = auditService;
        }


        public async Task<DeliveryResponseDto> CreateAsync(CreateDeliveryDto createDelivery)
        {
            var createValidation = await _createDeliveryValidator.ValidateAsync(createDelivery);

            if (!createValidation.IsValid)
            {
                throw new ApplicationValidationException(
                    createValidation.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var salesOrder = await _salesOrderRepository.GetByIdAsync(createDelivery.SalesOrderId);

            if (salesOrder is null)
            {
                throw new NotFoundException($"Sales order with ID {createDelivery.SalesOrderId} was not found.");
            }

            var delivery = new Delivery
            {
                SalesOrderId = createDelivery.SalesOrderId,
                DriverName = createDelivery.DriverName,
                VehiclePlate = createDelivery.VehiclePlate,
                DeliveryAddress = createDelivery.DeliveryAddress,
                Notes = createDelivery.Notes,

                Status = DeliveryStatus.Pending
            };

            await _deliveryRepository.AddAsync(delivery);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Create,
                Entity = AuditEntity.Delivery,
                EntityId = delivery.Id.ToString(),
                NewValues = JsonSerializer.Serialize(delivery),
                Success = true
            });

            return await GetByIdAsync(delivery.Id);
        }

        public async Task<DeliveryResponseDto> UpdateAsync(int id, UpdateDeliveryDto updateDelivery)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid delivery id.");
            }

            var validationUpdate = await _updateDeliveryValidator.ValidateAsync(updateDelivery);

            if (!validationUpdate.IsValid)
            {
                throw new ApplicationValidationException(
                    validationUpdate.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var delivery = await _deliveryRepository.GetByIdAsync(id);

            if (delivery is null)
            {
                throw new NotFoundException($"Delivery with ID {id} was not found.");
            }

            var oldValues = JsonSerializer.Serialize(delivery);

            delivery.DriverName = updateDelivery.DriverName;
            delivery.VehiclePlate = updateDelivery.VehiclePlate;
            delivery.DeliveryAddress = updateDelivery.DeliveryAddress;
            delivery.Notes = updateDelivery.Notes;

            await _deliveryRepository.UpdateAsync(delivery);

            var newValues = JsonSerializer.Serialize(delivery);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.Delivery,
                EntityId = delivery.Id.ToString(),
                OldValues = oldValues,
                NewValues = newValues,
                Success = true
            });

            return await GetByIdAsync(delivery.Id);
        }


        public async Task<DeliveryResponseDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid delivery id.");
            }

            var delivery = await _deliveryRepository.GetByIdAsync(id);

            if (delivery is null)
            {
                throw new NotFoundException($"Delivery with ID {id} was not found.");
            }

            return new DeliveryResponseDto
            {
                Id = delivery.Id,
                SalesOrderId = delivery.SalesOrderId,
                OrderNumber = delivery.SalesOrder.OrderNumber,
                Status = delivery.Status,
                DriverName = delivery.DriverName,
                VehiclePlate = delivery.VehiclePlate,
                DeliveryAddress = delivery.DeliveryAddress,
                DepartureDate = delivery.DepartureDate,
                DeliveredAt = delivery.DeliveredAt,
                Notes = delivery.Notes
            };
        }


        public async Task<IEnumerable<DeliveryResponseDto>> GetAllAsync()
        {
            var deliveries = await _deliveryRepository.GetAllAsync();

            return deliveries.Select(delivery => new DeliveryResponseDto
            {
                Id = delivery.Id,
                SalesOrderId = delivery.SalesOrderId,
                OrderNumber = delivery.SalesOrder.OrderNumber,
                Status = delivery.Status,
                DriverName = delivery.DriverName,
                VehiclePlate = delivery.VehiclePlate,
                DeliveryAddress = delivery.DeliveryAddress,
                DepartureDate = delivery.DepartureDate,
                DeliveredAt = delivery.DeliveredAt,
                Notes = delivery.Notes
            });
        }


        public async Task<IEnumerable<DeliveryResponseDto>> GetByStatusAsync(DeliveryStatus deliveryStatus)
        {
            if (!Enum.IsDefined(typeof(DeliveryStatus), deliveryStatus))
            {
                throw new BadRequestException("Invalid delivery status.");
            }

            var deliveries = await _deliveryRepository.GetByStatusAsync(deliveryStatus);

            if (!deliveries.Any())
            {
                throw new NotFoundException($"No deliveries found with status {deliveryStatus}.");
            }

            return deliveries.Select(delivery => new DeliveryResponseDto
            {
                Id = delivery.Id,
                SalesOrderId = delivery.SalesOrderId,
                OrderNumber = delivery.SalesOrder.OrderNumber,
                Status = delivery.Status,
                DriverName = delivery.DriverName,
                VehiclePlate = delivery.VehiclePlate,
                DeliveryAddress = delivery.DeliveryAddress,
                DepartureDate = delivery.DepartureDate,
                DeliveredAt = delivery.DeliveredAt,
                Notes = delivery.Notes
            });


        }


        public async Task<DeliveryResponseDto> UpdateStatusAsync(int id,UpdateDeliveryStatusDto updateDeliveryStatus)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid delivery id.");
            }

            if (!Enum.IsDefined(typeof(DeliveryStatus), updateDeliveryStatus.Status))
            {
                throw new BadRequestException("Invalid delivery status.");
            }

            var delivery = await _deliveryRepository.GetByIdAsync(id);

            if (delivery is null)
            {
                throw new NotFoundException($"Delivery with ID {id} was not found.");
            }

            var oldValues = JsonSerializer.Serialize(delivery);

            delivery.Status = updateDeliveryStatus.Status;

            await _deliveryRepository.UpdateAsync(delivery);

            var newValues = JsonSerializer.Serialize(delivery);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.Delivery,
                EntityId = delivery.Id.ToString(),
                OldValues = oldValues,
                NewValues = newValues,
                Success = true
            });

            return await GetByIdAsync(delivery.Id);
        }


        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid delivery id.");
            }

            var delivery = await _deliveryRepository.GetByIdAsync(id);

            if (delivery is null)
            {
                throw new NotFoundException($"Delivery with ID {id} was not found.");
            }

            var oldValues = JsonSerializer.Serialize(delivery);

            await _deliveryRepository.DeleteAsync(delivery);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Delete,
                Entity = AuditEntity.Delivery,
                EntityId = delivery.Id.ToString(),
                OldValues = oldValues,
                Success = true
            });
        }
    }
}