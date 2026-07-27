using StockFlow.Application.Common.Pagination;
using StockFlow.Application.DTOs.Catalog.Product;
using StockFlow.Domain.Entities.Catalog;
using StockFlow.Domain.Enums.Catalog;

namespace StockFlow.Application.Interfaces.Catalog
{
    public interface IProductRepository
    {
        // PRODUCT (core)
        Task<PagedResult<Product>> GetAllAsync(ProductQueryParametersDto queryParameters);

        Task<Product?> GetByIdAsync(int id);

        Task<Product?> GetBySKUAsync(string sku);

        Task<bool> SKUExistsAsync(string sku, int? ignoreId = null);

        // FILTERS
        Task<IEnumerable<Product>> GetLowStockAsync();

        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);

        Task<IEnumerable<Product>> GetBySupplierAsync(int supplierId);

        // PRODUCT ITEMS

        Task<Product?> GetWithItemsAsync(int productId);

        Task<ProductItem?> GetItemByIdAsync(int id);

        Task<ProductItem?> GetItemBySerialAsync(string serialNumber);

        Task<IEnumerable<ProductItem>> GetItemsByProductIdAsync(int productId);

        Task<IEnumerable<ProductItem>> GetItemsByVariantIdAsync(int variantId);

        Task AddProductItemAsync(ProductItem item);

        Task UpdateProductItemStatusAsync(int productItemId, ProductItemStatus status);

        Task<IEnumerable<ProductItem>> GetItemsByStatusAsync(ProductItemStatus status);

        // PRODUCT VARIANTS
        Task<ProductVariant?> GetVariantByIdAsync(int id);

        Task<IEnumerable<ProductVariant>> GetVariantsByProductIdAsync(int productId);
        Task<IEnumerable<ProductVariant>> GetAllVariantsAsync();

        Task AddProductVariantAsync(ProductVariant variant);

        Task UpdateProductVariantAsync(ProductVariant variant);

        Task DeleteProductVariantAsync(ProductVariant variant);

        // WRITE
        Task AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(Product product);
    }
}