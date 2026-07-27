using StockFlow.Application.DTOs.Catalog.ProductVariant;

namespace StockFlow.Application.Interfaces.Catalog.IServices
{
    public interface IProductVariantService
    {
        Task<ProductVariantResponseDto> CreateAsync(CreateProductVariantDto createProductVariant);

        Task<ProductVariantResponseDto> UpdateAsync(int id, UpdateProductVariantDto updateProductVariant);

        Task<ProductVariantResponseDto> GetByIdAsync(int productVariantId);

        Task<IEnumerable<ProductVariantResponseDto>> GetByProductIdAsync(int productId);

        Task<IEnumerable<ProductVariantResponseDto>> GetAllAsync();

        Task ActivateAsync(int id);

        Task DeactivateAsync(int id);

        Task DeleteAsync(int id);
    }
}

