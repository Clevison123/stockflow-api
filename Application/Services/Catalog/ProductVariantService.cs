using FluentValidation;
using StockFlow.Application.DTOs.Audit;
using StockFlow.Application.DTOs.Catalog.ProductVariant;
using StockFlow.Application.Exceptions;
using StockFlow.Application.Interfaces.Catalog;
using StockFlow.Application.Interfaces.Catalog.IServices;
using StockFlow.Application.Interfaces.IAudit;
using StockFlow.Application.Interfaces.Identity;
using StockFlow.Domain.Entities.Catalog;
using StockFlow.Domain.Enums.Audit;
using System.Text.Json;

namespace StockFlow.Application.Services.Catalog
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<CreateProductVariantDto> _createProductVariantValidation;
        private readonly IValidator<UpdateProductVariantDto> _updateProductVariantValidation;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuditService _auditService;

        public ProductVariantService(
            IProductRepository productRepository,
            IValidator<CreateProductVariantDto> createProductVariantValidation,
            IValidator<UpdateProductVariantDto> updateProductVariantValidation,
            ICurrentUserService currentUserService,
            IAuditService auditService)
        {
            _productRepository = productRepository;
            _createProductVariantValidation = createProductVariantValidation;
            _updateProductVariantValidation = updateProductVariantValidation;
            _currentUserService = currentUserService;
            _auditService = auditService;
        }


        public async Task<ProductVariantResponseDto> CreateAsync(CreateProductVariantDto createProductVariant)
        {
            var validation = await _createProductVariantValidation.ValidateAsync(createProductVariant);

            if (!validation.IsValid)
            {
                throw new ApplicationValidationException(
                    validation.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var product = await _productRepository.GetByIdAsync(createProductVariant.ProductId);

            if (product is null)
            {
                throw new NotFoundException("Product not found.");
            }

            var productVariant = new ProductVariant
            {
                ProductId = createProductVariant.ProductId,
                Name = createProductVariant.Name,
                Color = createProductVariant.Color,
                Size = createProductVariant.Size,
                Storage = createProductVariant.Storage,
                Memory = createProductVariant.Memory,
                SalePrice = createProductVariant.SalePrice
            };

            await _productRepository.AddProductVariantAsync(productVariant);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Create,
                Entity = AuditEntity.ProductVariant,
                EntityId = productVariant.Id.ToString(),
                NewValues = JsonSerializer.Serialize(productVariant),
                Success = true
            });

            return await GetByIdAsync(productVariant.Id);
        }

        public async Task<ProductVariantResponseDto> GetByIdAsync(int productVariantId)
        {
            if (productVariantId <= 0)
            {
                throw new BadRequestException("Invalid product variant id.");
            }

            var productVariant = await _productRepository.GetVariantByIdAsync(productVariantId);

            if (productVariant is null)
            {
                throw new NotFoundException($"Product variant with ID {productVariantId} was not found.");
            }

            return new ProductVariantResponseDto
            {
                Id = productVariant.Id,
                ProductId = productVariant.ProductId,
                ProductName = productVariant.Product.Name,
                Name = productVariant.Name,
                Color = productVariant.Color,
                Size = productVariant.Size,
                Storage = productVariant.Storage,
                Memory = productVariant.Memory,
                SalePrice = productVariant.SalePrice,
                IsActive = productVariant.IsActive
            };
        }

        public async Task<IEnumerable<ProductVariantResponseDto>> GetByProductIdAsync(int productId)
        {
            if (productId <= 0)
            {
                throw new BadRequestException("Invalid product id.");
            }

            var productVariants = await _productRepository.GetVariantsByProductIdAsync(productId);

            if (!productVariants.Any())
            {
                throw new NotFoundException($"No variants found for product ID {productId}.");
            }

            return productVariants.Select(productVariant => new ProductVariantResponseDto
            {
                Id = productVariant.Id,
                ProductId = productVariant.ProductId,
                ProductName = productVariant.Product.Name,
                Name = productVariant.Name,
                Color = productVariant.Color,
                Size = productVariant.Size,
                Storage = productVariant.Storage,
                Memory = productVariant.Memory,
                SalePrice = productVariant.SalePrice,
                IsActive = productVariant.IsActive
            });
        }

        public async Task<ProductVariantResponseDto> UpdateAsync(int id, UpdateProductVariantDto updateProductVariant)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid product variant id.");
            }

            var validation = await _updateProductVariantValidation.ValidateAsync(updateProductVariant);

            if (!validation.IsValid)
            {
                throw new ApplicationValidationException(
                    validation.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var productVariant = await _productRepository.GetVariantByIdAsync(id);

            if (productVariant is null)
            {
                throw new NotFoundException($"Product variant with ID {id} was not found.");
            }

            var oldValues = JsonSerializer.Serialize(productVariant);

            productVariant.Name = updateProductVariant.Name;
            productVariant.Color = updateProductVariant.Color;
            productVariant.Size = updateProductVariant.Size;
            productVariant.Storage = updateProductVariant.Storage;
            productVariant.Memory = updateProductVariant.Memory;
            productVariant.SalePrice = updateProductVariant.SalePrice;

            await _productRepository.UpdateProductVariantAsync(productVariant);

            var newValues = JsonSerializer.Serialize(productVariant);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.ProductVariant,
                EntityId = productVariant.Id.ToString(),
                OldValues = oldValues,
                NewValues = newValues,
                Success = true
            });

            return await GetByIdAsync(productVariant.Id);
        }

        public async Task ActivateAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid product variant id.");
            }

            var productVariant = await _productRepository.GetVariantByIdAsync(id);

            if (productVariant is null)
            {
                throw new NotFoundException($"Product variant with ID {id} was not found.");
            }

            if (productVariant.IsActive)
            {
                throw new BusinessRuleException("The product variant is already active.");
            }

            var oldValues = JsonSerializer.Serialize(productVariant);

            productVariant.IsActive = true;

            await _productRepository.UpdateProductVariantAsync(productVariant);

            var newValues = JsonSerializer.Serialize(productVariant);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.ProductVariant,
                EntityId = productVariant.Id.ToString(),
                OldValues = oldValues,
                NewValues = newValues,
                Success = true
            });
        }

        public async Task DeactivateAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid product variant id.");
            }

            var productVariant = await _productRepository.GetVariantByIdAsync(id);

            if (productVariant is null)
            {
                throw new NotFoundException($"Product variant with ID {id} was not found.");
            }

            if (!productVariant.IsActive)
            {
                throw new BusinessRuleException("The product variant is already inactive.");
            }

            var oldValues = JsonSerializer.Serialize(productVariant);

            productVariant.IsActive = false;

            await _productRepository.UpdateProductVariantAsync(productVariant);

            var newValues = JsonSerializer.Serialize(productVariant);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.ProductVariant,
                EntityId = productVariant.Id.ToString(),
                OldValues = oldValues,
                NewValues = newValues,
                Success = true
            });
        }

        public async Task<IEnumerable<ProductVariantResponseDto>> GetAllAsync()
        {
            var productVariants = await _productRepository.GetAllVariantsAsync();

            return productVariants.Select(productVariant => new ProductVariantResponseDto
            {
                Id = productVariant.Id,
                ProductId = productVariant.ProductId,
                ProductName = productVariant.Product.Name,
                Name = productVariant.Name,
                Color = productVariant.Color,
                Size = productVariant.Size,
                Storage = productVariant.Storage,
                Memory = productVariant.Memory,
                SalePrice = productVariant.SalePrice,
                IsActive = productVariant.IsActive
            });
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid product variant id.");
            }

            var productVariant = await _productRepository.GetVariantByIdAsync(id);

            if (productVariant is null)
            {
                throw new NotFoundException($"Product variant with ID {id} was not found.");
            }

            var oldValues = JsonSerializer.Serialize(productVariant);

            await _productRepository.DeleteProductVariantAsync(productVariant);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Delete,
                Entity = AuditEntity.ProductVariant,
                EntityId = productVariant.Id.ToString(),
                OldValues = oldValues,
                Success = true
            });
        }
    }
}
