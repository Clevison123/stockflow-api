using StockFlow.Application.DTOs.Catalog.ProductItem;
using StockFlow.Domain.Enums;

namespace StockFlow.Application.Interfaces.Catalog.IServices
{
    public interface IProductItemService
    {
        Task<ProductItemResponseDto>
            GetByIdAsync(int id);

        Task<ProductItemResponseDto>
            GetBySerialNumberAsync(string serialNumber);

        Task<IEnumerable<ProductItemResponseDto>>
            GetByProductIdAsync(int productId);

        Task UpdateStatusAsync(
            int productItemId,
            ProductItemStatus status);

        Task DeactivateAsync(int id);
    }
}
