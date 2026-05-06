using StockFlow.API.Domain.Entities;
using StockFlow.API.DTOs.StockMovement;

namespace StockFlow.API.Application.Interfaces
{
    public interface IStockMovementService
    {
        Task<StockMovement> RegisterEntryAsync(CreateStockMovementDto dto);
        Task<StockMovement> RegisterExitAsync(CreateStockMovementDto dto);
        Task<IEnumerable<StockMovement>> GetAllAsync();
        Task<IEnumerable<StockMovement>> GetByProductIdAsync(int productId);
    }
}
