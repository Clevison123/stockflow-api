using StockFlow.Domain.Entities.Catalog;
using StockFlow.Domain.Enums;

namespace StockFlow.Application.Interfaces.Catalog
{
    public interface IProductRepository
    {
        // PRODUCT (core)
        Task<IEnumerable<Product>> GetAllAsync(string? search);

        Task<Product?> GetByIdAsync(int id);

        Task<Product?> GetBySKUAsync(string sku);

        Task<bool> SKUExistsAsync(string sku, int? ignoreId = null);

        // FILTERS
        Task<IEnumerable<Product>> GetLowStockAsync();

        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);

        Task<IEnumerable<Product>> GetBySupplierAsync(int supplierId);

        // PRODUCT ITEMS (sem repository próprio)

        Task<Product?> GetWithItemsAsync(int productId);

        Task<ProductItem?> GetItemBySerialAsync(string serialNumber);

        Task AddProductItemAsync(int productId, ProductItem item);

        Task UpdateProductItemStatusAsync(int productItemId, ProductItemStatus status);

        Task<IEnumerable<ProductItem>> GetItemsByStatusAsync(ProductItemStatus status);

        // WRITE
        Task AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(Product product);
    }
}