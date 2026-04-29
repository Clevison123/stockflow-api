using StockFlow.API.DTOs.Product;
using StockFlow.API.Entities;
using StockFlow.API.Helpers;

namespace StockFlow.API.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(CreateProductDto dto);
        Task<PagedResult<Product>> GetAllProductsAsync(ProductQueryParameters queryParameters);
        Task<Product> GetProductByIdOrThrowAsync(int id);
        Task<Product> UpdateProductAsync(int id, UpdateProductDto dto);
        Task DeleteProductAsync(int id);
    }
}