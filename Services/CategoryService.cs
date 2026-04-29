using StockFlow.API.Data;
using StockFlow.API.DTOs.Category;
using StockFlow.API.Entities;
using StockFlow.API.Exceptions;
using StockFlow.API.Interfaces; 
using System.Text.Json;       

namespace StockFlow.API.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;

        private readonly IAuditService _auditService;
        private readonly ICurrentUserService _currentUserService;

        public CategoryService(AppDbContext context,
                               IAuditService auditService,
                               ICurrentUserService currentUserService)
        {
            _context = context;
            _auditService = auditService;
            _currentUserService = currentUserService;
        }

        public async Task<Category> CreateCategoryAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            await _auditService.LogAsync(new AuditLog
            {
                UserId = _currentUserService.UserId,
                UserEmail = _currentUserService.Email,
                Action = "CREATE",
                EntityName = "Category",
                EntityId = category.Id.ToString(),
                NewValues = JsonSerializer.Serialize(category)
            });

            return category;
        }

        public List<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public Category? GetCategoryById(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
                throw new NotFoundException("Category not found.");

            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
                return null;

            var oldValues = JsonSerializer.Serialize(category);

            category.Name = dto.Name;
            category.Description = dto.Description;

            await _context.SaveChangesAsync();

            await _auditService.LogAsync(new AuditLog
            {
                UserId = _currentUserService.UserId,
                UserEmail = _currentUserService.Email,
                Action = "UPDATE",
                EntityName = "Category",
                EntityId = category.Id.ToString(),
                OldValues = oldValues,
                NewValues = JsonSerializer.Serialize(category)
            });

            return category;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
                return false;

            var oldValues = JsonSerializer.Serialize(category);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            await _auditService.LogAsync(new AuditLog
            {
                UserId = _currentUserService.UserId,
                UserEmail = _currentUserService.Email,
                Action = "DELETE",
                EntityName = "Category",
                EntityId = category.Id.ToString(),
                OldValues = oldValues
            });

            return true;
        }
    }
}