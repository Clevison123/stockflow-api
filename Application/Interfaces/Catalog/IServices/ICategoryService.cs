using StockFlow.Application.Common.Pagination;
using StockFlow.Application.DTOs.Catalog.Category;
using StockFlow.Application.DTOs.Catalog.Product;

namespace StockFlow.Application.Interfaces.Catalog.IServices
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto dto);

        Task<CategoryResponseDto> UpdateAsync(int id,UpdateCategoryDto updateCategory);

        Task<CategoryResponseDto> GetByIdAsync(int id);

        Task<PagedResult<CategoryResponseDto>> GetAllAsync(CategoryQueryParametersDto query);

        Task ActivateAsync(int id);

        Task DeactivateAsync(int id);
    }
}
