using StockFlow.API.Application.Common.Pagination;
using StockFlow.API.Application.DTOs.Product;
using StockFlow.API.Domain.Entities;
using StockFlow.API.DTOs.Product;

namespace StockFlow.API.Application.Interfaces
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