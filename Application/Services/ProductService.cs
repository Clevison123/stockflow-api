using Microsoft.EntityFrameworkCore;
using StockFlow.API.Application.Exceptions;
using System.Text.Json;
using StockFlow.Application.Common.Pagination;
using StockFlow.Infrastructure.Data;
using StockFlow.Domain.Entities.Catalog;
using StockFlow.Application.DTOs.Catalog.Product;
using StockFlow.Application.Interfaces.Identity;
using StockFlow.Application.Interfaces.Catalog;

namespace StockFlow.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        private readonly IAuditService _auditService;

        public ProductService(AppDbContext context,
                              ICurrentUserService currentUserService,
                              IAuditService auditService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _auditService = auditService;
        }

        public async Task<Product> CreateProductAsync(CreateProductDto dto)
        {
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
            var supplierExists = await _context.Suppliers.AnyAsync(s => s.Id == dto.SupplierId);

            if (!categoryExists)
                throw new NotFoundException($"Category with ID {dto.CategoryId} was not found.");

            if (!supplierExists)
                throw new NotFoundException($"Supplier with ID {dto.SupplierId} was not found.");

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                QuantityInStock = dto.QuantityInStock,
                MinimumStock = dto.MinimumStock,
                CategoryId = dto.CategoryId,
                SupplierId = dto.SupplierId,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = _currentUserService.UserId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            await _auditService.LogAsync(new AuditLog
            {
                UserId = _currentUserService.UserId,
                UserEmail = _currentUserService.Email,
                Action = "CREATE",
                EntityName = "Product",
                EntityId = product.Id.ToString(),
                NewValues = JsonSerializer.Serialize(product)
            });

            return await GetProductByIdOrThrowAsync(product.Id);
        }

        public async Task<PagedResult<Product>> GetAllProductsAsync(ProductQueryParameters queryParameters)
        {
            var query = _context.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParameters.Search))
            {
                var search = queryParameters.Search.Trim().ToLower();

                query = query.Where(p =>
                    p.Name.ToLower().Contains(search) ||
                    p.Description.ToLower().Contains(search));
            }

            if (queryParameters.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == queryParameters.CategoryId.Value);
            }

            if (queryParameters.SupplierId.HasValue)
            {
                query = query.Where(p => p.SupplierId == queryParameters.SupplierId.Value);
            }

            query = (queryParameters.SortBy?.ToLower(), queryParameters.SortDirection?.ToLower()) switch
            {
                ("price", "desc") => query.OrderByDescending(p => p.Price),
                ("price", "asc") => query.OrderBy(p => p.Price),

                ("quantityinstock", "desc") => query.OrderByDescending(p => p.QuantityInStock),
                ("quantityinstock", "asc") => query.OrderBy(p => p.QuantityInStock),

                ("minimumstock", "desc") => query.OrderByDescending(p => p.MinimumStock),
                ("minimumstock", "asc") => query.OrderBy(p => p.MinimumStock),

                ("createdat", "desc") => query.OrderByDescending(p => p.CreatedAt),
                ("createdat", "asc") => query.OrderBy(p => p.CreatedAt),

                ("name", "desc") => query.OrderByDescending(p => p.Name),
                _ => query.OrderBy(p => p.Name)
            };

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync();

            return new PagedResult<Product>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize
            };
        }

        public async Task<Product> GetProductByIdOrThrowAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product == null)
                throw new NotFoundException($"Product with ID {id} was not found.");

            return product;
        }

        public async Task<Product> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var product = await GetProductByIdOrThrowAsync(id);

            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
            var supplierExists = await _context.Suppliers.AnyAsync(s => s.Id == dto.SupplierId);

            if (!categoryExists)
                throw new NotFoundException($"Category with ID {dto.CategoryId} was not found.");

            if (!supplierExists)
                throw new NotFoundException($"Supplier with ID {dto.SupplierId} was not found.");

            var oldValues = JsonSerializer.Serialize(product);

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.QuantityInStock = dto.QuantityInStock;
            product.MinimumStock = dto.MinimumStock;
            product.CategoryId = dto.CategoryId;
            product.SupplierId = dto.SupplierId;
            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedByUserId = _currentUserService.UserId;

            await _context.SaveChangesAsync();

            await _auditService.LogAsync(new AuditLog
            {
                UserId = _currentUserService.UserId,
                UserEmail = _currentUserService.Email,
                Action = "UPDATE",
                EntityName = "Product",
                EntityId = product.Id.ToString(),
                OldValues = oldValues,
                NewValues = JsonSerializer.Serialize(product)
            });

            return product;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await GetProductByIdOrThrowAsync(id);

            var oldValues = JsonSerializer.Serialize(product);

            product.IsDeleted = true;
            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedByUserId = _currentUserService.UserId;

            await _context.SaveChangesAsync();

            await _auditService.LogAsync(new AuditLog
            {
                UserId = _currentUserService.UserId,
                UserEmail = _currentUserService.Email,
                Action = "DELETE",
                EntityName = "Product",
                EntityId = product.Id.ToString(),
                OldValues = oldValues
            });
        }
    }
}