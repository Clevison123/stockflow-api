using StockFlow.Domain.Entities.Inventory;
using StockFlow.Domain.Enums.Inventory;

namespace StockFlow.Application.Interfaces.Inventory
{
    public interface IStockMovementRepository
    {
        Task<StockMovement?> GetByIdAsync(int id);

        Task<IEnumerable<StockMovement>> GetAllAsync();

        Task<IEnumerable<StockMovement>> GetByProductIdAsync(int productId);

        Task<IEnumerable<StockMovement>> GetByTypeAsync(MovementType type);

        Task<IEnumerable<StockMovement>> GetRecentAsync(int count);

        Task AddAsync(StockMovement movement);

        Task DeleteAsync(StockMovement movement);
    }
}