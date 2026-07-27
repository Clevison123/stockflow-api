using StockFlow.Application.Common.Pagination;
using StockFlow.Application.DTOs.Catalog.Product;
using StockFlow.Domain.Entities.Catalog;

namespace StockFlow.Application.Interfaces.Catalog.IServices
{
    public interface IProductService
    {
        Task<ProductResponseDto> CreateAsync(CreateProductDto dto);

        Task<ProductResponseDto> UpdateAsync(int id,UpdateProductDto dto);

        Task<ProductResponseDto> GetByIdAsync(int id);

        Task<PagedResult<ProductResponseDto>> GetAllAsync(ProductQueryParametersDto query);

        Task ActivateAsync(int id);

        Task DeactivateAsync(int id);

        Task<IEnumerable<LowStockProductDto>>GetLowStockAsync();

        Task<CurrentStockProductDto> GetCurrentStockAsync(int productId);
    }
}