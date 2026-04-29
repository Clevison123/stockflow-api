using StockFlow.API.DTOs.StockMovement;
using StockFlow.API.Entities;

namespace StockFlow.API.Interfaces
{
    public interface IStockMovementService
    {
        Task<StockMovement> RegisterEntryAsync(CreateStockMovementDto dto);
        Task<StockMovement> RegisterExitAsync(CreateStockMovementDto dto);
        Task<IEnumerable<StockMovement>> GetAllAsync();
        Task<IEnumerable<StockMovement>> GetByProductIdAsync(int productId);
    }
}
