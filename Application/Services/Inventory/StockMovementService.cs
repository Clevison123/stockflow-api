using FluentValidation;
using StockFlow.Application.DTOs.Audit;
using StockFlow.Application.DTOs.Inventory.StockMovement;
using StockFlow.Application.Exceptions;
using StockFlow.Application.Interfaces.Catalog;
using StockFlow.Application.Interfaces.IAudit;
using StockFlow.Application.Interfaces.Identity;
using StockFlow.Application.Interfaces.Inventory;
using StockFlow.Domain.Entities.Inventory;
using StockFlow.Domain.Enums.Audit;
using StockFlow.Domain.Enums.Inventory;
using System.Text.Json;

namespace StockFlow.Application.Services.Inventory
{
    public class StockMovementService : IStockMovementService
    {
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAuditService _auditService;
        private readonly ICurrentUserService _currentUserService;

        private readonly IValidator<CreateStockMovementDto> _createValidator;

        public StockMovementService(
            IStockMovementRepository stockMovementRepository,
            IProductRepository productRepository,
            IAuditService auditService,
            ICurrentUserService currentUserService,
            IValidator<CreateStockMovementDto> createValidator)
        {
            _stockMovementRepository = stockMovementRepository;
            _productRepository = productRepository;
            _auditService = auditService;
            _currentUserService = currentUserService;

            _createValidator = createValidator;
        }

        public async Task<StockMovementResponseDto> RegisterEntryAsync(CreateStockMovementDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var product = await _productRepository.GetByIdAsync(dto.ProductId);

            if (product is null)
            {
                throw new NotFoundException($"Product with ID {dto.ProductId} was not found.");
            }

            var previousQuantity = product.QuantityInStock;
            var currentQuantity = previousQuantity + dto.Quantity;

            product.QuantityInStock = currentQuantity;

            await _productRepository.UpdateAsync(product);

            var movement = new StockMovement
            {
                ProductId = product.Id,
                MovementType = MovementType.Entry,
                Quantity = dto.Quantity,
                PreviousQuantity = previousQuantity,
                CurrentQuantity = currentQuantity,
                Reason = dto.Reason,
                PerformedByUserId = _currentUserService.UserId
            };

            await _stockMovementRepository.AddAsync(movement);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Create,
                Entity = AuditEntity.StockMovement,
                EntityId = movement.Id.ToString(),
                NewValues = JsonSerializer.Serialize(movement),
                Success = true
            });

            return new StockMovementResponseDto
            {
                Id = movement.Id,
                ProductId = product.Id,
                ProductName = product.Name,
                MovementType = movement.MovementType,
                Quantity = movement.Quantity,
                PreviousQuantity = movement.PreviousQuantity,
                CurrentQuantity = movement.CurrentQuantity,
                Reason = movement.Reason,
                PerformedByUserId = movement.PerformedByUserId,
                PerformedByUserName = null,
                CreatedAt = movement.CreatedAt
            };
        }

        public async Task<StockMovementResponseDto> RegisterAdjustmentAsync(CreateStockMovementDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var product = await _productRepository.GetByIdAsync(dto.ProductId);

            if (product is null)
            {
                throw new NotFoundException($"Product with ID {dto.ProductId} was not found.");
            }

            var previousQuantity = product.QuantityInStock;
            var currentQuantity = dto.Quantity;
            var adjustedQuantity = Math.Abs(currentQuantity - previousQuantity);

            product.QuantityInStock = currentQuantity;

            await _productRepository.UpdateAsync(product);

            var movement = new StockMovement
            {
                ProductId = product.Id,
                MovementType = MovementType.Adjustment,
                Quantity = adjustedQuantity,
                PreviousQuantity = previousQuantity,
                CurrentQuantity = currentQuantity,
                PerformedByUserId = _currentUserService.UserId,
                Reason = dto.Reason
            };

            await _stockMovementRepository.AddAsync(movement);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Create,
                Entity = AuditEntity.StockMovement,
                EntityId = movement.Id.ToString(),
                NewValues = JsonSerializer.Serialize(movement),
                Success = true
            });

            return new StockMovementResponseDto
            {
                Id = movement.Id,
                ProductId = product.Id,
                ProductName = product.Name,
                MovementType = movement.MovementType,
                Quantity = movement.Quantity,
                PreviousQuantity = movement.PreviousQuantity,
                CurrentQuantity = movement.CurrentQuantity,
                Reason = movement.Reason,
                PerformedByUserId = movement.PerformedByUserId,
                CreatedAt = movement.CreatedAt
            };
        }

        public async Task<StockMovementResponseDto> RegisterExitAsync(CreateStockMovementDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var product = await _productRepository.GetByIdAsync(dto.ProductId);

            if (product is null)
            {
                throw new NotFoundException($"Product with ID {dto.ProductId} was not found.");
            }

            if (product.QuantityInStock < dto.Quantity)
            {
                throw new BadRequestException("Insufficient stock to complete this movement.");
            }

            var previousQuantity = product.QuantityInStock;
            var currentQuantity = previousQuantity - dto.Quantity;

            product.QuantityInStock = currentQuantity;

            await _productRepository.UpdateAsync(product);

            var movement = new StockMovement
            {
                ProductId = product.Id,
                MovementType = MovementType.Exit,
                Quantity = dto.Quantity,
                PreviousQuantity = previousQuantity,
                CurrentQuantity = currentQuantity,
                PerformedByUserId = _currentUserService.UserId,
                Reason = dto.Reason
            };

            await _stockMovementRepository.AddAsync(movement);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Create,
                Entity = AuditEntity.StockMovement,
                EntityId = movement.Id.ToString(),
                NewValues = JsonSerializer.Serialize(movement),
                Success = true
            });

            return new StockMovementResponseDto
            {
                Id = movement.Id,
                ProductId = product.Id,
                ProductName = product.Name,
                MovementType = movement.MovementType,
                Quantity = movement.Quantity,
                PreviousQuantity = movement.PreviousQuantity,
                CurrentQuantity = movement.CurrentQuantity,
                Reason = movement.Reason,
                PerformedByUserId = movement.PerformedByUserId,
                PerformedByUserName = null, 
                CreatedAt = movement.CreatedAt
            };
        }

        public async Task<IEnumerable<StockMovementResponseDto>> GetByProductIdAsync(int productId)
        {
            if (productId <= 0)
            {
                throw new BadRequestException("Invalid product ID.");
            }

            var product = await _productRepository.GetByIdAsync(productId);

            if (product is null)
            {
                throw new NotFoundException($"Product with ID {productId} was not found.");
            }

            var movements = await _stockMovementRepository.GetByProductIdAsync(productId);

            if (!movements.Any())
            {
                throw new NotFoundException($"No stock movements were found for product ID {productId}.");
            }

            return movements.Select(movement => new StockMovementResponseDto
            {
                Id = movement.Id,
                ProductId = movement.ProductId,
                ProductName = product.Name,
                MovementType = movement.MovementType,
                Quantity = movement.Quantity,
                PreviousQuantity = movement.PreviousQuantity,
                CurrentQuantity = movement.CurrentQuantity,
                Reason = movement.Reason,
                PerformedByUserId = movement.PerformedByUserId,
                PerformedByUserName = null,
                CreatedAt = movement.CreatedAt
            });
        }

        public async Task<IEnumerable<StockMovementResponseDto>> GetAllAsync()
        {
            var movements = await _stockMovementRepository.GetAllAsync();

            if (!movements.Any())
            {
                throw new NotFoundException("No stock movements were found.");
            }

            var response = new List<StockMovementResponseDto>();

            foreach (var movement in movements)
            {
                var product = await _productRepository.GetByIdAsync(movement.ProductId);

                response.Add(new StockMovementResponseDto
                {
                    Id = movement.Id,
                    ProductId = movement.ProductId,
                    ProductName = product?.Name ?? string.Empty,
                    MovementType = movement.MovementType,
                    Quantity = movement.Quantity,
                    PreviousQuantity = movement.PreviousQuantity,
                    CurrentQuantity = movement.CurrentQuantity,
                    Reason = movement.Reason,
                    PerformedByUserId = movement.PerformedByUserId,
                    PerformedByUserName = null,
                    CreatedAt = movement.CreatedAt
                });
            }

            return response;
        }

        public async Task<StockMovementResponseDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid stock movement ID.");
            }

            var movement = await _stockMovementRepository.GetByIdAsync(id);

            if (movement is null)
            {
                throw new NotFoundException($"Stock movement with ID {id} was not found.");
            }

            var product = await _productRepository.GetByIdAsync(movement.ProductId);

            return new StockMovementResponseDto
            {
                Id = movement.Id,
                ProductId = movement.ProductId,
                ProductName = product?.Name ?? string.Empty,
                MovementType = movement.MovementType,
                Quantity = movement.Quantity,
                PreviousQuantity = movement.PreviousQuantity,
                CurrentQuantity = movement.CurrentQuantity,
                Reason = movement.Reason,
                PerformedByUserId = movement.PerformedByUserId,
                PerformedByUserName = null,
                CreatedAt = movement.CreatedAt
            };
        }
    }
}
