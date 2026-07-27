using FluentValidation;
using StockFlow.Application.DTOs.Audit;
using StockFlow.Application.DTOs.Catalog.ProductItem;
using StockFlow.Application.DTOs.Catalog.ProductVariant;
using StockFlow.Application.Exceptions;
using StockFlow.Application.Interfaces.Catalog;
using StockFlow.Application.Interfaces.Catalog.IServices;
using StockFlow.Application.Interfaces.IAudit;
using StockFlow.Application.Interfaces.Identity;
using StockFlow.Domain.Enums.Audit;
using StockFlow.Domain.Enums.Catalog;
using System.Text.Json;
using ProductItemResponseDto = StockFlow.Application.DTOs.Catalog.ProductItem.ProductItemResponseDto;

namespace StockFlow.Application.Services.Catalog
{
    public class ProductItemService : IProductItemService
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<UpdateProductItemStatusDto> _updateProductItemStatusValidator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuditService _auditService;

        public ProductItemService(
            IProductRepository productRepository,
            ICurrentUserService currentUserService,
            IAuditService auditService,
            IValidator<UpdateProductItemStatusDto> updateProductItemStatusValidator)
        {
            _productRepository = productRepository;
            _currentUserService = currentUserService;
            _auditService = auditService;
            _updateProductItemStatusValidator = updateProductItemStatusValidator;
        }

        public async Task<ProductItemResponseDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid product id.");
            }

            var productItem = await _productRepository.GetItemByIdAsync(id);

            if (productItem is null)
            {
                throw new NotFoundException($"Product with ID {id} was not found.");
            }

            return new ProductItemResponseDto
            {
                Id = productItem.Id,
                ProductName = productItem.Product.Name,
                SerialNumber = productItem.SerialNumber,
                Variant = new ProductVariantSummaryDto
                {
                    Id = productItem.ProductVariant.Id,
                    Name = productItem.ProductVariant.Name
                },
                Status = productItem.Status,
                ReceivedAt = productItem.ReceivedAt,
                WarrantyUntil = productItem.WarrantyUntil
            };
        }

        public async Task<ProductItemResponseDto> GetBySerialNumberAsync(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
            {
                throw new BadRequestException("Serial number is required.");
            }

            var productItem = await _productRepository.GetItemBySerialAsync(serialNumber);

            if (productItem is null)
            {
                throw new NotFoundException($"Product item with Serial Number: {serialNumber} was not found.");
            }

            return new ProductItemResponseDto
            {
                Id = productItem.Id,
                ProductName = productItem.Product.Name,
                SerialNumber = productItem.SerialNumber,
                Status = productItem.Status,
                Variant = new ProductVariantSummaryDto
                {
                    Id = productItem.ProductVariant.Id,
                    Name = productItem.ProductVariant.Name
                },
                ReceivedAt = productItem.ReceivedAt,
                WarrantyUntil = productItem.WarrantyUntil
            };
        }

        public async Task UpdateStatusAsync(int productItemId, UpdateProductItemStatusDto updateDto)
        {
            var validation = await _updateProductItemStatusValidator.ValidateAsync(updateDto);

            if (!validation.IsValid)
            {
                throw new ApplicationValidationException(
                    validation.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var productItem = await _productRepository.GetItemByIdAsync(productItemId);

            if (productItem is null)
            {
                throw new NotFoundException($"Product item with ID {productItemId} was not found.");
            }

            if (productItem.Status == ProductItemStatus.Lost &&  updateDto.Status == ProductItemStatus.InStock)
            {
                throw new BusinessRuleException(
                    "A lost item cannot be returned to stock.");
            }

            var oldValues = JsonSerializer.Serialize(productItem);

            productItem.Status = updateDto.Status;

            var newValues = JsonSerializer.Serialize(productItem);

            await _productRepository.UpdateProductItemStatusAsync(productItemId, updateDto.Status);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.ProductItem,
                EntityId = productItem.Id.ToString(),
                OldValues = oldValues,
                NewValues = newValues,
                Success = true
            });
        }

        public async Task<IEnumerable<ProductItemResponseDto>> GetByVariantIdAsync(int variantId)
        {
            if (variantId <= 0)
            {
                throw new BadRequestException("Invalid product variant id.");
            }

            var variantItems = await _productRepository.GetItemsByVariantIdAsync(variantId);

            return variantItems.Select(variants => new ProductItemResponseDto
            {
                Id = variants.Id,
                ProductName = variants.Product.Name,

                Variant = new ProductVariantSummaryDto
                {
                    Id = variants.ProductVariant.Id,
                    Name = variants.ProductVariant.Name
                },

                SerialNumber = variants.SerialNumber,
                Status = variants.Status,
                ReceivedAt = variants.ReceivedAt,
                WarrantyUntil = variants.WarrantyUntil
            });
        }

        public async Task<IEnumerable<ProductItemResponseDto>> GetByProductIdAsync(int productId)
        {
            if (productId <= 0)
            {
                throw new BadRequestException("Invalid product item id.");
            }

            var productItems = await _productRepository.GetItemsByProductIdAsync(productId);

            return productItems.Select(item => new ProductItemResponseDto
            {
                Id = item.Id,
                ProductName = item.Product.Name,

                Variant = new ProductVariantSummaryDto
                {
                    Id = item.ProductVariant.Id,
                    Name = item.ProductVariant.Name
                },

                SerialNumber = item.SerialNumber,
                Status = item.Status,
                ReceivedAt = item.ReceivedAt,
                WarrantyUntil = item.WarrantyUntil
            });
        }
    }
}
