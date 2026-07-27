using FluentValidation;
using StockFlow.Application.Common.Pagination;
using StockFlow.Application.DTOs.Audit;
using StockFlow.Application.DTOs.Catalog.Category;
using StockFlow.Application.Exceptions;
using StockFlow.Application.Interfaces.Catalog;
using StockFlow.Application.Interfaces.Catalog.IServices;
using StockFlow.Application.Interfaces.IAudit;
using StockFlow.Application.Interfaces.Identity;
using StockFlow.Domain.Entities.Catalog;
using StockFlow.Domain.Enums.Audit;
using System.Text.Json;

namespace StockFlow.Application.Services.Catalog
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuditService _auditService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidator<CreateCategoryDto> _createValidation;
        private readonly IValidator<UpdateCategoryDto> _updateValidation;
        private readonly IValidator<CategoryQueryParametersDto> _categoryQueryValidation;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IAuditService auditService,
            ICurrentUserService currentUserService,
            IValidator<CreateCategoryDto> createValidation,
            IValidator<UpdateCategoryDto> updateValidation,
            IValidator<CategoryQueryParametersDto> categoryQueryValidation)
        {
            _categoryRepository = categoryRepository;
            _auditService = auditService;
            _currentUserService = currentUserService;
            _createValidation = createValidation;
            _updateValidation = updateValidation;
            _categoryQueryValidation = categoryQueryValidation;
        }

        public async Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto createCategory)
        {
            var createCategoryValidation = await _createValidation.ValidateAsync(createCategory);

            if (!createCategoryValidation.IsValid)
            {
                throw new ApplicationValidationException(
                    createCategoryValidation.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var exists = await _categoryRepository.NameExistsAsync(createCategory.Name);

            if (exists)
            {
                throw new ConflictException("Category already exists.");
            }

            var category = new Category
            {
                Name = createCategory.Name,
                Description = createCategory.Description,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = _currentUserService.UserId
            };

            await _categoryRepository.AddAsync(category);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Create,
                Entity = AuditEntity.Category,
                EntityId = category.Id.ToString(),
                NewValues = JsonSerializer.Serialize(category),
                Success = true
            });

            return await GetByIdAsync(category.Id);
        }

        public async Task<PagedResult<CategoryResponseDto>> GetAllAsync(CategoryQueryParametersDto queryCategoryParameters)
        {
            var searchValidation = await _categoryQueryValidation.ValidateAsync(queryCategoryParameters);

            if (!searchValidation.IsValid)
            {
                throw new ApplicationValidationException(searchValidation.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var searchCategories = await _categoryRepository.GetAllAsync(queryCategoryParameters);

            var response = new PagedResult<CategoryResponseDto>
            {
                Items = searchCategories.Items.Select(category => new CategoryResponseDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    IsActive = category.IsActive,
                    CreatedAt = category.CreatedAt
                }).ToList(),

                TotalCount = searchCategories.TotalCount,
                PageNumber = searchCategories.PageNumber,
                PageSize = searchCategories.PageSize
            };

            return response;
        }

        public async Task<CategoryResponseDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid category id.");
            }

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                throw new NotFoundException($"Category with ID {id} was not found.");
            }

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt
            };
        }

        public async Task<CategoryResponseDto> UpdateAsync(int id, UpdateCategoryDto updateCategory)
        {
            var validationUpdate = await _updateValidation.ValidateAsync(updateCategory);

            if (!validationUpdate.IsValid)
            {
                throw new ApplicationValidationException(validationUpdate.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                throw new NotFoundException($"Category with ID {id} was not found.");
            }

            category.Name = updateCategory.Name;
            category.Description = updateCategory.Description;
            category.UpdatedAt = DateTime.UtcNow;
            category.UpdatedByUserId = _currentUserService.UserId;

            await _categoryRepository.UpdateAsync(category);

            return await GetByIdAsync(category.Id);
        }

        public async Task ActivateAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid category id.");
            }

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                throw new NotFoundException($"Category with ID {id} was not found.");
            }

            if (category.IsActive)
            {
                throw new ConflictException("Category is already active.");
            }

            category.IsActive = true;
            category.UpdatedAt = DateTime.UtcNow;
            category.UpdatedByUserId = _currentUserService.UserId;

            await _categoryRepository.UpdateAsync(category);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Activate,
                Entity = AuditEntity.Category,
                EntityId = category.Id.ToString(),
                NewValues = JsonSerializer.Serialize(category),
                Success = true
            });
        }

        public async Task DeactivateAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid category id.");
            }

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                throw new NotFoundException($"Category with ID {id} was not found.");
            }

            if (!category.IsActive)
            {
                throw new ConflictException("Category is already inactive.");
            }

            category.IsActive = false;
            category.UpdatedAt = DateTime.UtcNow;
            category.UpdatedByUserId = _currentUserService.UserId;

            await _categoryRepository.UpdateAsync(category);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Deactivate,
                Entity = AuditEntity.Category,
                EntityId = category.Id.ToString(),
                NewValues = JsonSerializer.Serialize(category),
                Success = true
            });
        }
    }
}