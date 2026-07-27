using FluentValidation;
using StockFlow.Application.Common.Pagination;
using StockFlow.Application.DTOs.Audit;
using StockFlow.Application.DTOs.Catalog.Product;
using StockFlow.Application.Exceptions;
using StockFlow.Application.Interfaces.Catalog;
using StockFlow.Application.Interfaces.Catalog.IServices;
using StockFlow.Application.Interfaces.IAudit;
using StockFlow.Application.Interfaces.Identity;
using StockFlow.Application.Interfaces.Purchasing;
using StockFlow.Domain.Entities.Catalog;
using StockFlow.Domain.Enums.Audit;
using System.Text.Json;

namespace StockFlow.Application.Services.Catalog
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuditService _auditService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IValidator<CreateProductDto> _createValidator;
        private readonly IValidator<UpdateProductDto> _updateValidator;
        private readonly IValidator<ProductQueryParametersDto> _queryValidator;

        public ProductService(
            IProductRepository productRepository,
            ICurrentUserService currentUserService,
            IAuditService auditService,
            ICategoryRepository categoryRepository,
            ISupplierRepository supplierRepository,
            IValidator<CreateProductDto> createValidator,
            IValidator<UpdateProductDto> updateValidator,
            IValidator<ProductQueryParametersDto> queryValidator)
        {
            _productRepository = productRepository;
            _currentUserService = currentUserService;
            _auditService = auditService;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _queryValidator = queryValidator;
        }

        public async Task<ProductResponseDto> CreateAsync(CreateProductDto createProduct)
        {
            var validationCreation = await _createValidator.ValidateAsync(createProduct);

            if (!validationCreation.IsValid)
            {
                throw new ApplicationValidationException(validationCreation.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var skuExists = await _productRepository.SKUExistsAsync(createProduct.SKU);

            if (skuExists)
            {
                throw new ConflictException($"The SKU '{createProduct.SKU}' is already registered.");
            }

            var category = await _categoryRepository.GetByIdAsync(createProduct.CategoryId);
            var supplier = await _supplierRepository.GetByIdAsync(createProduct.SupplierId);

            if (category is null)
                throw new NotFoundException($"Category with ID {createProduct.CategoryId} was not found.");

            if (supplier is null)
                throw new NotFoundException($"Supplier with ID {createProduct.SupplierId} was not found.");

            var product = new Product
            {
                Name = createProduct.Name,
                Description = createProduct.Description,
                SKU = createProduct.SKU,
                Barcode = createProduct.Barcode,
                Brand = createProduct.Brand,
                Model = createProduct.Model,
                OriginCountry = createProduct.OriginCountry,
                WarrantyMonths = createProduct.WarrantyMonths,
                HasSerialNumber = createProduct.HasSerialNumber,
                PurchasePrice = createProduct.PurchasePrice,
                SalePrice = createProduct.SalePrice,
                QuantityInStock = createProduct.QuantityInStock,
                MinimumStock = createProduct.MinimumStock,
                CategoryId = createProduct.CategoryId,
                SupplierId = createProduct.SupplierId,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = _currentUserService.UserId
            };

            await _productRepository.AddAsync(product);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Create,
                Entity = AuditEntity.Product,
                EntityId = product.Id.ToString(),
                NewValues = JsonSerializer.Serialize(product),
                Success = true
            });

            return await GetByIdAsync(product.Id);
        }

        public async Task<PagedResult<ProductResponseDto>> GetAllAsync(ProductQueryParametersDto queryParameters)
        {

            var searchValidation = await _queryValidator.ValidateAsync(queryParameters);

            if (!searchValidation.IsValid)
            {
                throw new ApplicationValidationException(searchValidation.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var searchProducts = await _productRepository.GetAllAsync(queryParameters);

            var response = new PagedResult<ProductResponseDto>
            {
                Items = searchProducts.Items.Select(product => new ProductResponseDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    SKU = product.SKU,
                    Barcode = product.Barcode,
                    Brand = product.Brand,
                    Model = product.Model,
                    OriginCountry = product.OriginCountry,
                    WarrantyMonths = product.WarrantyMonths,
                    HasSerialNumber = product.HasSerialNumber,
                    PurchasePrice = product.PurchasePrice,
                    SalePrice = product.SalePrice,
                    QuantityInStock = product.QuantityInStock,
                    MinimumStock = product.MinimumStock,
                    IsActive = product.IsActive,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category?.Name ?? string.Empty,
                    SupplierId = product.SupplierId,
                    SupplierName = product.Supplier?.Name ?? string.Empty,
                    CreatedAt = product.CreatedAt
                }).ToList(),

                TotalCount = searchProducts.TotalCount,
                PageNumber = searchProducts.PageNumber,
                PageSize = searchProducts.PageSize
            };

            return response;

        }

        public async Task<ProductResponseDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid product id.");
            }

            var product = await _productRepository.GetByIdAsync(id);

            if (product is null)
            {
                throw new NotFoundException($"Product with ID {id} was not found.");
            }

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU,
                Barcode = product.Barcode,
                Brand = product.Brand,
                Model = product.Model,
                OriginCountry = product.OriginCountry,
                WarrantyMonths = product.WarrantyMonths,
                HasSerialNumber = product.HasSerialNumber,
                PurchasePrice = product.PurchasePrice,
                SalePrice = product.SalePrice,
                QuantityInStock = product.QuantityInStock,
                MinimumStock = product.MinimumStock,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? string.Empty,
                SupplierId = product.SupplierId,
                SupplierName = product.Supplier?.Name ?? string.Empty,
                CreatedAt = product.CreatedAt
            };
        }

        public async Task<ProductResponseDto> UpdateAsync(int id, UpdateProductDto update)
        {
            var validationUpdate = await _updateValidator.ValidateAsync(update);

            if (!validationUpdate.IsValid)
            {
                throw new ApplicationValidationException(validationUpdate.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var product = await _productRepository.GetByIdAsync(id);

            if (product is null)
            {
                throw new NotFoundException($"Product with ID {id} was not found.");
            }

            product.Name = update.Name;
            product.Description = update.Description;
            product.Barcode = update.Barcode;
            product.Brand = update.Brand;
            product.Model = update.Model;
            product.OriginCountry = update.OriginCountry;
            product.WarrantyMonths = update.WarrantyMonths;
            product.HasSerialNumber = update.HasSerialNumber;
            product.PurchasePrice = update.PurchasePrice;
            product.SalePrice = update.SalePrice;
            product.QuantityInStock = update.QuantityInStock;
            product.MinimumStock = update.MinimumStock;
            product.CategoryId = update.CategoryId;
            product.SupplierId = update.SupplierId;

            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedByUserId = _currentUserService.UserId;

            await _productRepository.UpdateAsync(product);

            return await GetByIdAsync(product.Id);
        }

        public async Task ActivateAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid product id.");
            }

            var product = await _productRepository.GetByIdAsync(id);

            if (product is null)
            {
                throw new NotFoundException($"Product with ID {id} was not found.");
            }

            if (product.IsActive)
            {
                throw new ConflictException("Product is already active.");
            }

            product.IsActive = true;
            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedByUserId = _currentUserService.UserId;

            await _productRepository.UpdateAsync(product);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Activate, 
                Entity = AuditEntity.Product,
                EntityId = product.Id.ToString(),
                NewValues = JsonSerializer.Serialize(product),
                Success = true
            });
        }

        public async Task DeactivateAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid product id.");
            }

            var product = await _productRepository.GetByIdAsync(id);

            if (product is null)
            {
                throw new NotFoundException($"Product with ID {id} was not found.");
            }

            if (!product.IsActive)
            {
                throw new ConflictException("Product is already inactive.");
            }

            product.IsActive = false;
            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedByUserId = _currentUserService.UserId;

            await _productRepository.UpdateAsync(product);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Deactivate,
                Entity = AuditEntity.Product,
                EntityId = product.Id.ToString(),
                NewValues = JsonSerializer.Serialize(product),
                Success = true
            });
        }

        public async Task<IEnumerable<LowStockProductDto>> GetLowStockAsync()
        {
            var products = await _productRepository.GetLowStockAsync();

            if (!products.Any())
            {
                throw new NotFoundException("There are no products with low stock.");
            }

            return products.Select(product => new LowStockProductDto
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                QuantityInStock = product.QuantityInStock,
                MinimumStock = product.MinimumStock,
                CategoryName = product.Category?.Name ?? string.Empty,
                SupplierName = product.Supplier?.Name ?? string.Empty,
                IsActive = product.IsActive
            });

        }

        public async Task<CurrentStockProductDto> GetCurrentStockAsync(int productId)
        {
            if (productId <= 0)
            {
                throw new BadRequestException("Invalid product id.");
            }

            var currentStock = await _productRepository.GetByIdAsync(productId);

            if (currentStock is null)
            {
                throw new NotFoundException($"Product with ID {productId} was not found.");
            }

            return new CurrentStockProductDto
            {
                Id = currentStock.Id,
                Name = currentStock.Name,
                SKU = currentStock.SKU,
                QuantityInStock = currentStock.QuantityInStock,
                MinimumStock = currentStock.MinimumStock,
                CategoryName = currentStock.Category?.Name ?? string.Empty,
                SupplierName = currentStock.Supplier?.Name ?? string.Empty,
                IsActive = currentStock.IsActive,
                IsLowStock = currentStock.QuantityInStock <= currentStock.MinimumStock,
                LastUpdatedAt = currentStock.UpdatedAt ?? currentStock.CreatedAt
            };
        }
    }
}