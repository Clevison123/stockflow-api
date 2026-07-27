using FluentValidation;
using StockFlow.Application.DTOs.Audit;
using StockFlow.Application.DTOs.SupplierClaim;
using StockFlow.Application.Exceptions;
using StockFlow.Application.Interfaces.IAudit;
using StockFlow.Application.Interfaces.Identity;
using StockFlow.Application.Interfaces.Purchasing;
using StockFlow.Application.Interfaces.Quality;
using StockFlow.Domain.Entities.Quality;
using StockFlow.Domain.Enums.Audit;
using System.Text.Json;

namespace StockFlow.Application.Services.Quality
{
    public class SupplierClaimService : ISupplierClaimService
    {
        private readonly ISupplierClaimRepository _supplierClaimRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuditService _auditService;
        private readonly IQualityIssueRepository _qualityIssueRepository;

        private readonly IValidator<CreateSupplierClaimDto> _createValidator;
        private readonly IValidator<UpdateSupplierClaimDto> _updateValidator;
        private readonly IValidator<ResolveSupplierClaimDto> _resolveValidator;

        public SupplierClaimService(
            ISupplierClaimRepository supplierClaimRepository,
            ISupplierRepository supplierRepository,
            IQualityIssueRepository qualityIssueRepository,
            ICurrentUserService currentUserService,
            IAuditService auditService,
            IValidator<CreateSupplierClaimDto> createValidator,
            IValidator<UpdateSupplierClaimDto> updateValidator,
            IValidator<ResolveSupplierClaimDto> resolveValidator)
        {
            _supplierClaimRepository = supplierClaimRepository;
            _supplierRepository = supplierRepository;
            _qualityIssueRepository = qualityIssueRepository;
            _currentUserService = currentUserService;
            _auditService = auditService;

            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _resolveValidator = resolveValidator;
        }

        public async Task<SupplierClaimResponseDto> CreateAsync(CreateSupplierClaimDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
            {
                throw new UnauthorizedException("Authenticated user is required to create a supplier claim.");
            }

            var supplier = await _supplierRepository.GetByIdAsync(dto.SupplierId);

            if (supplier is null)
            {
                throw new NotFoundException($"Supplier with ID {dto.SupplierId} was not found.");
            }

            if (dto.QualityIssueId.HasValue)
            {
                var qualityIssue = await _qualityIssueRepository.GetByIdAsync(dto.QualityIssueId.Value);

                if (qualityIssue is null)
                {
                    throw new NotFoundException($"Quality issue with ID {dto.QualityIssueId.Value} was not found.");
                }
            }

            var claim = new SupplierClaim
            {
                SupplierId = dto.SupplierId,
                ClaimType = dto.ClaimType,
                Description = dto.Description,
                OpenedAt = DateTime.UtcNow,
                OpenedByUserId = _currentUserService.UserId.Value,
                QualityIssueId = dto.QualityIssueId,
                IsResolved = false
            };

            await _supplierClaimRepository.AddAsync(claim);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Create,
                Entity = AuditEntity.SupplierClaim,
                EntityId = claim.Id.ToString(),
                NewValues = JsonSerializer.Serialize(claim),
                Success = true
            });

            return await GetByIdAsync(claim.Id);
        }

        public async Task<IEnumerable<SupplierClaimResponseDto>> GetAllAsync()
        {
            var claims = await _supplierClaimRepository.GetAllAsync();

            if (!claims.Any())
            {
                throw new NotFoundException("No supplier claims were found.");
            }

            return claims.Select(claim => new SupplierClaimResponseDto
            {
                Id = claim.Id,
                SupplierId = claim.SupplierId,
                SupplierName = claim.Supplier.Name,
                ClaimType = claim.ClaimType,
                Description = claim.Description,
                OpenedAt = claim.OpenedAt,
                OpenedByUserId = claim.OpenedByUserId,
                OpenedByUserName = claim.OpenedByUser.FullName,
                IsResolved = claim.IsResolved,
                ResolvedAt = claim.ResolvedAt,
                ResolutionNotes = claim.ResolutionNotes,
                QualityIssueId = claim.QualityIssueId
            });
        }

        public async Task<SupplierClaimResponseDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid supplier claim ID.");
            }

            var claim = await _supplierClaimRepository.GetByIdAsync(id);

            if (claim is null)
            {
                throw new NotFoundException($"Supplier claim with ID {id} was not found.");
            }

            return new SupplierClaimResponseDto
            {
                Id = claim.Id,
                SupplierId = claim.SupplierId,
                SupplierName = claim.Supplier.Name,
                ClaimType = claim.ClaimType,
                Description = claim.Description,
                OpenedAt = claim.OpenedAt,
                OpenedByUserId = claim.OpenedByUserId,
                OpenedByUserName = claim.OpenedByUser.FullName,
                IsResolved = claim.IsResolved,
                ResolvedAt = claim.ResolvedAt,
                ResolutionNotes = claim.ResolutionNotes,
                QualityIssueId = claim.QualityIssueId
            };
        }

        public async Task<IEnumerable<SupplierClaimResponseDto>> GetByQualityIssueAsync(int qualityIssueId)
        {
            if (qualityIssueId <= 0)
            {
                throw new BadRequestException("Invalid quality issue ID.");
            }

            var qualityIssue = await _qualityIssueRepository.GetByIdAsync(qualityIssueId);

            if (qualityIssue is null)
            {
                throw new NotFoundException($"Quality issue with ID {qualityIssueId} was not found.");
            }

            var claims = await _supplierClaimRepository.GetByQualityIssueIdAsync(qualityIssueId);

            if (!claims.Any())
            {
                throw new NotFoundException(
                    $"No supplier claims found for quality issue ID {qualityIssueId}.");
            }

            return claims.Select(claim => new SupplierClaimResponseDto
            {
                Id = claim.Id,
                SupplierId = claim.SupplierId,
                SupplierName = claim.Supplier.Name,
                ClaimType = claim.ClaimType,
                Description = claim.Description,
                OpenedAt = claim.OpenedAt,
                OpenedByUserId = claim.OpenedByUserId,
                OpenedByUserName = claim.OpenedByUser.FullName,
                IsResolved = claim.IsResolved,
                ResolvedAt = claim.ResolvedAt,
                ResolutionNotes = claim.ResolutionNotes,
                QualityIssueId = claim.QualityIssueId
            });
        }

        public async Task<IEnumerable<SupplierClaimResponseDto>> GetBySupplierAsync(int supplierId)
        {
            if (supplierId <= 0)
            {
                throw new BadRequestException("Invalid supplier ID.");
            }

            var supplier = await _supplierRepository.GetByIdAsync(supplierId);

            if (supplier is null)
            {
                throw new NotFoundException($"Supplier with ID {supplierId} was not found.");
            }

            var claims = await _supplierClaimRepository.GetBySupplierIdAsync(supplierId);

            if (!claims.Any())
            {
                throw new NotFoundException($"No supplier claims found for supplier ID {supplierId}.");
            }

            return claims.Select(claim => new SupplierClaimResponseDto
            {
                Id = claim.Id,
                SupplierId = claim.SupplierId,
                SupplierName = claim.Supplier.Name,
                ClaimType = claim.ClaimType,
                Description = claim.Description,
                OpenedAt = claim.OpenedAt,
                OpenedByUserId = claim.OpenedByUserId,
                OpenedByUserName = claim.OpenedByUser.FullName,
                IsResolved = claim.IsResolved,
                ResolvedAt = claim.ResolvedAt,
                ResolutionNotes = claim.ResolutionNotes,
                QualityIssueId = claim.QualityIssueId
            });
        }

        public async Task<IEnumerable<SupplierClaimResponseDto>> GetOpenClaimsAsync()
        {
            var claims = await _supplierClaimRepository.GetOpenClaimsAsync();

            if (!claims.Any())
            {
                throw new NotFoundException("No open supplier claims were found.");
            }

            return claims.Select(claim => new SupplierClaimResponseDto
            {
                Id = claim.Id,
                SupplierId = claim.SupplierId,
                SupplierName = claim.Supplier.Name,
                ClaimType = claim.ClaimType,
                Description = claim.Description,
                OpenedAt = claim.OpenedAt,
                OpenedByUserId = claim.OpenedByUserId,
                OpenedByUserName = claim.OpenedByUser.FullName,
                IsResolved = claim.IsResolved,
                ResolvedAt = claim.ResolvedAt,
                ResolutionNotes = claim.ResolutionNotes,
                QualityIssueId = claim.QualityIssueId
            });
        }

        public async Task<IEnumerable<SupplierClaimResponseDto>> GetResolvedClaimsAsync()
        {
            var resolvedClaims = await _supplierClaimRepository.GetResolvedClaimsAsync();

            if (!resolvedClaims.Any())
            {
                throw new NotFoundException("No resolved supplier claims were found.");
            }

            return resolvedClaims.Select(claim => new SupplierClaimResponseDto
            {
                Id = claim.Id,

                SupplierId = claim.SupplierId,
                SupplierName = claim.Supplier.Name,
                ClaimType = claim.ClaimType,
                Description = claim.Description,
                OpenedAt = claim.OpenedAt,
                OpenedByUserId = claim.OpenedByUserId,
                OpenedByUserName = claim.OpenedByUser.FullName,
                IsResolved = claim.IsResolved,
                ResolvedAt = claim.ResolvedAt,
                ResolutionNotes = claim.ResolutionNotes,
                QualityIssueId = claim.QualityIssueId
            });
        }

        public async Task ResolveAsync(int claimId, ResolveSupplierClaimDto dto)
        {
            if (claimId <= 0)
            {
                throw new BadRequestException("Invalid supplier claim ID.");
            }

            var validationResult = await _resolveValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var claim = await _supplierClaimRepository.GetByIdAsync(claimId);

            if (claim is null)
            {
                throw new NotFoundException($"Supplier claim with ID {claimId} was not found.");
            }

            if (claim.IsResolved)
            {
                throw new BadRequestException("Supplier claim is already resolved.");
            }

            claim.IsResolved = true;
            claim.ResolvedAt = DateTime.UtcNow;
            claim.ResolutionNotes = dto.ResolutionNotes;

            await _supplierClaimRepository.UpdateAsync(claim);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.SupplierClaim,
                EntityId = claim.Id.ToString(),
                NewValues = JsonSerializer.Serialize(claim),
                Success = true
            });
        }

        public async Task<SupplierClaimResponseDto> UpdateAsync(int id, UpdateSupplierClaimDto dto)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid supplier claim ID.");
            }

            var validationResult = await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(
                    validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var claim = await _supplierClaimRepository.GetByIdAsync(id);

            if (claim is null)
            {
                throw new NotFoundException($"Supplier claim with ID {id} was not found.");
            }

            claim.ClaimType = dto.ClaimType;
            claim.Description = dto.Description;

            await _supplierClaimRepository.UpdateAsync(claim);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.SupplierClaim,
                EntityId = claim.Id.ToString(),
                NewValues = JsonSerializer.Serialize(claim),
                Success = true
            });

            return new SupplierClaimResponseDto
            {
                Id = claim.Id,
                SupplierId = claim.SupplierId,
                SupplierName = claim.Supplier.Name,
                ClaimType = claim.ClaimType,
                Description = claim.Description,
                OpenedAt = claim.OpenedAt,
                OpenedByUserId = claim.OpenedByUserId,
                OpenedByUserName = claim.OpenedByUser.FullName,
                IsResolved = claim.IsResolved,
                ResolvedAt = claim.ResolvedAt,
                ResolutionNotes = claim.ResolutionNotes,
                QualityIssueId = claim.QualityIssueId
            };
        }
    }
}
