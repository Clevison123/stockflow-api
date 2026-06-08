using StockFlow.Application.DTOs.Catalog.Category;

namespace StockFlow.Application.Interfaces.Catalog
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> CreateAsync(
            CreateCategoryDto dto);

        Task<CategoryResponseDto> UpdateAsync(
            int id,
            UpdateCategoryDto dto);

        Task<CategoryResponseDto> GetByIdAsync(
            int id);

        Task<IEnumerable<CategoryResponseDto>>
            GetAllAsync();

        Task ActivateAsync(int id);

        Task DeactivateAsync(int id);
    }
}
