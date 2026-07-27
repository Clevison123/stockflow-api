using FluentValidation;
using StockFlow.Application.DTOs.Audit;
using StockFlow.Application.DTOs.Quality.QualityIssue;
using StockFlow.Application.Exceptions;
using StockFlow.Application.Interfaces.Catalog;
using StockFlow.Application.Interfaces.IAudit;
using StockFlow.Application.Interfaces.Identity;
using StockFlow.Application.Interfaces.Quality;
using StockFlow.Application.Interfaces.Quality.IServices;
using StockFlow.Domain.Entities.Quality;
using StockFlow.Domain.Enums.Audit;
using System.Text.Json;

namespace StockFlow.Application.Services.Quality
{
    public class QualityIssueService : IQualityIssueService
    {
        private readonly IQualityIssueRepository _qualityIssueRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuditService _auditService;

        private readonly IValidator<CreateQualityIssueDto> _createValidator;
        private readonly IValidator<UpdateQualityIssueDto> _updateValidator;
        private readonly IValidator<ResolveQualityIssueDto> _resolveValidator;


        public QualityIssueService(
            IQualityIssueRepository qualityIssueRepository,
            IProductRepository productRepository,
            ICurrentUserService currentUserService,
            IAuditService auditService,
            IValidator<CreateQualityIssueDto> createValidator,
            IValidator<UpdateQualityIssueDto> updateValidator,
            IValidator<ResolveQualityIssueDto> resolveValidator)
        {
            _qualityIssueRepository = qualityIssueRepository;
            _productRepository = productRepository;
            _currentUserService = currentUserService;
            _auditService = auditService;

            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _resolveValidator = resolveValidator;
        }
        public async Task<QualityIssueResponseDto> CreateAsync(CreateQualityIssueDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
            {
                throw new UnauthorizedException("Authenticated user is required to create a quality issue.");
            }

            var productItem = await _productRepository.GetItemByIdAsync(dto.ProductItemId);

            if (productItem is null)
            {
                throw new NotFoundException($"Product item with ID {dto.ProductItemId} was not found.");
            }

            var issue = new QualityIssue
            {
                ProductItemId = dto.ProductItemId,
                IssueType = dto.IssueType,
                Description = dto.Description,
                DetectedAt = DateTime.UtcNow,
                DetectedByUserId = _currentUserService.UserId.Value,
                RequiresSupplierClaim = dto.RequiresSupplierClaim,
                IsResolved = false
            };

            await _qualityIssueRepository.AddAsync(issue);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Create,
                Entity = AuditEntity.QualityIssue,
                EntityId = issue.Id.ToString(),
                NewValues = JsonSerializer.Serialize(issue),
                Success = true
            });

            return await GetByIdAsync(issue.Id);
        }

        public async Task<IEnumerable<QualityIssueResponseDto>> GetAllAsync()
        {
            var issues = await _qualityIssueRepository.GetAllAsync();

            if (!issues.Any())
            {
                throw new NotFoundException("No quality issues were found.");
            }

            return issues.Select(issue => new QualityIssueResponseDto
            {
                Id = issue.Id,
                ProductItemId = issue.ProductItemId,
                ProductName = issue.ProductItem.Product.Name,
                IssueType = issue.IssueType,
                Description = issue.Description,
                DetectedAt = issue.DetectedAt,
                DetectedByUserId = issue.DetectedByUserId,
                DetectedByUserName = issue.DetectedByUser.FullName,
                RequiresSupplierClaim = issue.RequiresSupplierClaim,
                IsResolved = issue.IsResolved,
                ResolvedAt = issue.ResolvedAt,
                ResolutionNotes = issue.ResolutionNotes
            });
        }

        public async Task<QualityIssueResponseDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid quality issue ID.");
            }

            var issue = await _qualityIssueRepository.GetByIdAsync(id);

            if (issue is null)
            {
                throw new NotFoundException($"Quality issue with ID {id} was not found.");
            }

            return new QualityIssueResponseDto
            {
                Id = issue.Id,
                ProductItemId = issue.ProductItemId,
                ProductName = issue.ProductItem.Product.Name,
                IssueType = issue.IssueType,
                Description = issue.Description,
                DetectedAt = issue.DetectedAt,
                DetectedByUserId = issue.DetectedByUserId,
                DetectedByUserName = issue.DetectedByUser.FullName,
                RequiresSupplierClaim = issue.RequiresSupplierClaim,
                IsResolved = issue.IsResolved,
                ResolvedAt = issue.ResolvedAt,
                ResolutionNotes = issue.ResolutionNotes
            };
        }

        public async Task<IEnumerable<QualityIssueResponseDto>> GetByProductItemAsync(int productItemId)
        {
            if (productItemId <= 0)
            {
                throw new BadRequestException("Invalid product item ID.");
            }

            var productItem = await _productRepository.GetItemByIdAsync(productItemId);

            if (productItem is null)
            {
                throw new NotFoundException($"Product item with ID {productItemId} was not found.");
            }

            var issues = await _qualityIssueRepository.GetByProductItemIdAsync(productItemId);

            if (!issues.Any())
            {
                throw new NotFoundException($"No quality issues found for product item ID {productItemId}.");
            }

            return issues.Select(issue => new QualityIssueResponseDto
            {
                Id = issue.Id,
                ProductItemId = issue.ProductItemId,
                ProductName = issue.ProductItem.Product.Name,
                IssueType = issue.IssueType,
                Description = issue.Description,
                DetectedAt = issue.DetectedAt,
                DetectedByUserId = issue.DetectedByUserId,
                DetectedByUserName = issue.DetectedByUser.FullName,
                RequiresSupplierClaim = issue.RequiresSupplierClaim,
                IsResolved = issue.IsResolved,
                ResolvedAt = issue.ResolvedAt,
                ResolutionNotes = issue.ResolutionNotes
            });
        }

        public async Task<IEnumerable<QualityIssueResponseDto>> GetOpenIssuesAsync()
        {
            var issues = await _qualityIssueRepository.GetOpenIssuesAsync();

            if (!issues.Any())
            {
                throw new NotFoundException("No open quality issues were found.");
            }

            return issues.Select(issue => new QualityIssueResponseDto
            {
                Id = issue.Id,
                ProductItemId = issue.ProductItemId,
                ProductName = issue.ProductItem.Product.Name,
                IssueType = issue.IssueType,
                Description = issue.Description,
                DetectedAt = issue.DetectedAt,
                DetectedByUserId = issue.DetectedByUserId,
                DetectedByUserName = issue.DetectedByUser.FullName,
                RequiresSupplierClaim = issue.RequiresSupplierClaim,
                IsResolved = issue.IsResolved,
                ResolvedAt = issue.ResolvedAt,
                ResolutionNotes = issue.ResolutionNotes
            });
        }

        public async Task<IEnumerable<QualityIssueResponseDto>> GetPendingSupplierClaimsAsync()
        {
            var issues = await _qualityIssueRepository.GetPendingSupplierClaimsAsync();

            if (!issues.Any())
            {
                throw new NotFoundException(
                    "No quality issues requiring supplier claims were found.");
            }

            return issues.Select(issue => new QualityIssueResponseDto
            {
                Id = issue.Id,
                ProductItemId = issue.ProductItemId,
                ProductName = issue.ProductItem.Product.Name,
                IssueType = issue.IssueType,
                Description = issue.Description,
                DetectedAt = issue.DetectedAt,
                DetectedByUserId = issue.DetectedByUserId,
                DetectedByUserName = issue.DetectedByUser.FullName,
                RequiresSupplierClaim = issue.RequiresSupplierClaim,
                IsResolved = issue.IsResolved,
                ResolvedAt = issue.ResolvedAt,
                ResolutionNotes = issue.ResolutionNotes
            });
        }

        public async Task<IEnumerable<QualityIssueResponseDto>> GetResolvedIssuesAsync()
        {
            var issues = await _qualityIssueRepository.GetResolvedIssuesAsync();

            if (!issues.Any())
            {
                throw new NotFoundException("No resolved quality issues were found.");
            }

            return issues.Select(issue => new QualityIssueResponseDto
            {
                Id = issue.Id,
                ProductItemId = issue.ProductItemId,
                ProductName = issue.ProductItem.Product.Name,
                IssueType = issue.IssueType,
                Description = issue.Description,
                DetectedAt = issue.DetectedAt,
                DetectedByUserId = issue.DetectedByUserId,
                DetectedByUserName = issue.DetectedByUser.FullName,
                RequiresSupplierClaim = issue.RequiresSupplierClaim,
                IsResolved = issue.IsResolved,
                ResolvedAt = issue.ResolvedAt,
                ResolutionNotes = issue.ResolutionNotes
            });
        }

        public async Task ResolveAsync(int issueId, ResolveQualityIssueDto dto)
        {
            if (issueId <= 0)
            {
                throw new BadRequestException("Invalid quality issue ID.");
            }

            var validationResult = await _resolveValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var issue = await _qualityIssueRepository.GetByIdAsync(issueId);

            if (issue is null)
            {
                throw new NotFoundException($"Quality issue with ID {issueId} was not found.");
            }

            if (issue.IsResolved)
            {
                throw new BadRequestException("Quality issue is already resolved.");
            }

            issue.IsResolved = true;
            issue.ResolvedAt = DateTime.UtcNow;
            issue.ResolutionNotes = dto.ResolutionNotes;

            await _qualityIssueRepository.UpdateAsync(issue);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.QualityIssue,
                EntityId = issue.Id.ToString(),
                NewValues = JsonSerializer.Serialize(issue),
                Success = true
            });
        }

        public async Task<QualityIssueResponseDto> UpdateAsync(int id, UpdateQualityIssueDto dto)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid quality issue ID.");
            }

            var validationResult = await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var issue = await _qualityIssueRepository.GetByIdAsync(id);

            if (issue is null)
            {
                throw new NotFoundException($"Quality issue with ID {id} was not found.");
            }

            issue.IssueType = dto.IssueType;
            issue.Description = dto.Description;
            issue.RequiresSupplierClaim = dto.RequiresSupplierClaim;

            await _qualityIssueRepository.UpdateAsync(issue);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.QualityIssue,
                EntityId = issue.Id.ToString(),
                NewValues = JsonSerializer.Serialize(issue),
                Success = true
            });

            return new QualityIssueResponseDto
            {
                Id = issue.Id,
                ProductItemId = issue.ProductItemId,
                ProductName = issue.ProductItem.Product.Name,
                IssueType = issue.IssueType,
                Description = issue.Description,
                DetectedAt = issue.DetectedAt,
                DetectedByUserId = issue.DetectedByUserId,
                DetectedByUserName = issue.DetectedByUser.FullName,
                RequiresSupplierClaim = issue.RequiresSupplierClaim,
                IsResolved = issue.IsResolved,
                ResolvedAt = issue.ResolvedAt,
                ResolutionNotes = issue.ResolutionNotes
            };
        }
    }
}
