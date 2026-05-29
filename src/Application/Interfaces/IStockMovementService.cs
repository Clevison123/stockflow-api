using StockFlow.API.src.Application.DTOs.StockMovement;
using StockFlow.API.src.Domain.Entities;

namespace StockFlow.API.src.Application.Interfaces
{
    public interface IStockMovementService
    {
        Task<StockMovement> RegisterEntryAsync(CreateStockMovementDto dto);
        Task<StockMovement> RegisterExitAsync(CreateStockMovementDto dto);
        Task<IEnumerable<StockMovement>> GetAllAsync();
        Task<IEnumerable<StockMovement>> GetByProductIdAsync(int productId);
    }
}
