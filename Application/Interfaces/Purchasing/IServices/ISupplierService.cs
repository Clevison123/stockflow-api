using StockFlow.Application.DTOs.Purchasing.Supplier;

namespace StockFlow.Application.Interfaces.Purchasing
{
    public interface ISupplierService
    {
        Task<SupplierResponseDto>CreateAsync( CreateSupplierDto dto);

        Task<SupplierResponseDto>UpdateAsync(int id,UpdateSupplierDto dto);

        Task<SupplierResponseDto>GetByIdAsync(int id);

        Task<IEnumerable<SupplierResponseDto>>GetAllAsync();

        Task ActivateAsync(int id);

        Task DeactivateAsync(int id);
        Task DeleteAsync(int id);
    }
}
