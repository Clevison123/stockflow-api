using StockFlow.Domain.Entities.Inventory;
using StockFlow.Domain.Enums;

namespace StockFlow.Application.Interfaces.Inventory
{
    public interface IStockMovementRepository
    {
        // READ
        Task<IEnumerable<StockMovement>> GetAllAsync();

        Task<IEnumerable<StockMovement>> GetByProductIdAsync(int productId);

        Task<IEnumerable<StockMovement>> GetByTypeAsync(MovementType type);

        Task<IEnumerable<StockMovement>> GetRecentAsync(int count);

        // WRITE
        Task AddAsync(StockMovement movement);

        Task DeleteAsync(StockMovement movement);
    }
}