using FluentValidation;
using StockFlow.Application.DTOs.Audit;
using StockFlow.Application.DTOs.Purchasing.Supplier;
using StockFlow.Application.Exceptions;
using StockFlow.Application.Interfaces.IAudit;
using StockFlow.Application.Interfaces.Identity;
using StockFlow.Application.Interfaces.Purchasing;
using StockFlow.Domain.Entities.Purchasing;
using StockFlow.Domain.Enums.Audit;
using System.Text.Json;

namespace StockFlow.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuditService _auditService;
        private readonly IValidator<CreateSupplierDto> _createValidator;
        private readonly IValidator<UpdateSupplierDto> _updateValidator;

        public SupplierService(
            ISupplierRepository supplierRepository,
            ICurrentUserService currentUserService,
            IAuditService auditService,
            IValidator<CreateSupplierDto> createValidator,
            IValidator<UpdateSupplierDto> updateValidator)
        {
            _supplierRepository = supplierRepository;
            _currentUserService = currentUserService;
            _auditService = auditService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<SupplierResponseDto> CreateAsync(CreateSupplierDto dto)
        {
            var createValidation = await _createValidator.ValidateAsync(dto);

            if (!createValidation.IsValid)
            {
                throw new ApplicationValidationException( createValidation.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var emailExists = await _supplierRepository.EmailExistsAsync(dto.Email);

            if (emailExists)
            {
                throw new BusinessRuleException("A supplier with this email already exists.");
            }

            var supplier = new Supplier
            {
                Name = dto.Name,
                ContactPerson = dto.ContactPerson,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                Website = dto.Website
            };

            await _supplierRepository.AddAsync(supplier);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Create,
                Entity = AuditEntity.Supplier,
                EntityId = supplier.Id.ToString(),
                NewValues = JsonSerializer.Serialize(supplier),
                Success = true
            });

            return await GetByIdAsync(supplier.Id);
        }

        public async Task<SupplierResponseDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid supplier id.");
            }

            var supplier = await _supplierRepository.GetByIdAsync(id);

            if (supplier is null)
            {
                throw new NotFoundException($"Supplier with ID {id} was not found.");
            }

            return new SupplierResponseDto
            {
                Id = supplier.Id,
                Name = supplier.Name,
                ContactPerson = supplier.ContactPerson,
                Email = supplier.Email,
                Phone = supplier.Phone,
                Address = supplier.Address,
                Website = supplier.Website,
                IsActive = supplier.IsActive
            };
        }

        public async Task<SupplierResponseDto> UpdateAsync(int id, UpdateSupplierDto dto)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid supplier id.");
            }

            var validationUpdate = await _updateValidator.ValidateAsync(dto);

            if (!validationUpdate.IsValid)
            {
                throw new ApplicationValidationException(
                    validationUpdate.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var emailExists = await _supplierRepository.EmailExistsAsync(dto.Email, id);

            if (emailExists)
            {
                throw new BusinessRuleException("A supplier with this email already exists.");
            }
            var supplier = await _supplierRepository.GetByIdAsync(id);

            if (supplier is null)
            {
                throw new NotFoundException($"Supplier with ID {id} was not found.");
            }

            var oldValues = JsonSerializer.Serialize(supplier);

            supplier.Name = dto.Name;
            supplier.ContactPerson = dto.ContactPerson;
            supplier.Email = dto.Email;
            supplier.Phone = dto.Phone;
            supplier.Address = dto.Address;
            supplier.Website = dto.Website;

            await _supplierRepository.UpdateAsync(supplier);

            var newValues = JsonSerializer.Serialize(supplier);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.Supplier,
                EntityId = supplier.Id.ToString(),
                OldValues = oldValues,
                NewValues = newValues,
                Success = true
            });

            return await GetByIdAsync(supplier.Id);
        }
        public async Task<IEnumerable<SupplierResponseDto>> GetAllAsync()
        {
            var suppliers = await _supplierRepository.GetAllAsync(null);

            return suppliers.Select(supplier => new SupplierResponseDto
            {
                Id = supplier.Id,
                Name = supplier.Name,
                ContactPerson = supplier.ContactPerson,
                Email = supplier.Email,
                Phone = supplier.Phone,
                Address = supplier.Address,
                Website = supplier.Website,
                IsActive = supplier.IsActive
            });
        }

        public async Task ActivateAsync(int id)
        {

            if (id <= 0)
            {
                throw new BadRequestException("Invalid supplier id.");
            }

            var supplier = await _supplierRepository.GetByIdAsync(id);

            if (supplier is null)
            {
                throw new NotFoundException($"Supplier with ID {id} was not found.");
            }

            if (supplier.IsActive)
            {
                throw new ConflictException("Supplier is already active.");
            }

            supplier.IsActive = true;
            supplier.UpdatedAt = DateTime.UtcNow;
            supplier.UpdatedByUserId = _currentUserService.UserId;

            await _supplierRepository.UpdateAsync(supplier);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Activate,
                Entity = AuditEntity.Supplier,
                EntityId = supplier.Id.ToString(),
                NewValues = JsonSerializer.Serialize(supplier),
                Success = true
            });
        }

        public async Task DeactivateAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid supplier id.");
            }

            var supplier = await _supplierRepository.GetByIdAsync(id);

            if (supplier is null)
            {
                throw new NotFoundException($"Supplier with ID {id} was not found.");
            }

            if (!supplier.IsActive)
            {
                throw new ConflictException("Supplier is already inactive.");
            }

            supplier.IsActive = false;
            supplier.UpdatedAt = DateTime.UtcNow;
            supplier.UpdatedByUserId = _currentUserService.UserId;

            await _supplierRepository.UpdateAsync(supplier);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Deactivate,
                Entity = AuditEntity.Supplier,
                EntityId = supplier.Id.ToString(),
                NewValues = JsonSerializer.Serialize(supplier),
                Success = true
            });
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid supplier id.");
            }

            var supplier = await _supplierRepository.GetByIdAsync(id);

            if (supplier is null)
            {
                throw new NotFoundException($"Supplier with ID {id} was not found.");
            }

            var oldValues = JsonSerializer.Serialize(supplier);

            await _supplierRepository.DeleteAsync(supplier);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Delete,
                Entity = AuditEntity.Supplier,
                EntityId = supplier.Id.ToString(),
                OldValues = oldValues,
                Success = true
            });
        }
    }
}