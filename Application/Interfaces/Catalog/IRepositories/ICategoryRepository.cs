using StockFlow.Domain.Entities.Catalog;

namespace StockFlow.Application.Interfaces.Catalog
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync(string? search);

        Task<Category?> GetByIdAsync(int id);

        Task<Category?> GetByNameAsync(string name);

        Task<bool> NameExistsAsync(string name, int? ignoreId = null);

        Task AddAsync(Category category);

        Task UpdateAsync(Category category);

        Task DeleteAsync(Category category);
    }
}