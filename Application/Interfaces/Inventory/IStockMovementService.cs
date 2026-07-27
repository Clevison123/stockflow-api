using StockFlow.Application.DTOs.Inventory.StockMovement;

namespace StockFlow.Application.Interfaces.Inventory
{
    public interface IStockMovementService
    {
        Task<StockMovementResponseDto> RegisterEntryAsync(CreateStockMovementDto dto);
        Task<StockMovementResponseDto> RegisterExitAsync(CreateStockMovementDto dto);
        Task<StockMovementResponseDto> RegisterAdjustmentAsync(CreateStockMovementDto dto);
        Task<StockMovementResponseDto> GetByIdAsync(int id);
        Task<IEnumerable<StockMovementResponseDto>> GetAllAsync();
        Task<IEnumerable<StockMovementResponseDto>> GetByProductIdAsync(int productId);
    }
}