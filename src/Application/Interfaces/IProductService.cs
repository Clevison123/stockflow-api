using StockFlow.API.src.Application.Common.Pagination;
using StockFlow.API.src.Application.DTOs.Product;
using StockFlow.API.src.Domain.Entities;

namespace StockFlow.API.src.Application.Interfaces
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