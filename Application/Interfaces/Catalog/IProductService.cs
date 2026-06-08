using StockFlow.Application.Common.Pagination;
using StockFlow.Application.DTOs.Catalog.Product;
using StockFlow.Domain.Entities.Catalog;

namespace StockFlow.Application.Interfaces.Catalog
{
    public interface IProductService
    {
        Task<ProductResponseDto> CreateAsync(
            CreateProductDto dto);

        Task<ProductResponseDto> UpdateAsync(
            int id,
            UpdateProductDto dto);

        Task<ProductResponseDto> GetByIdAsync(
            int id);

        Task<PagedResult<ProductResponseDto>> GetAllAsync(
            ProductQueryParameters query);

        Task ActivateAsync(int id);

        Task DeactivateAsync(int id);

        Task<IEnumerable<ProductResponseDto>>
            GetLowStockProductsAsync();

        Task<int> GetCurrentStockAsync(
            int productId);
    }
}