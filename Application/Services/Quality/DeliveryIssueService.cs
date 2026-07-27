using FluentValidation;
using StockFlow.Application.DTOs.Audit;
using StockFlow.Application.DTOs.Quality.DeliveryIssue;
using StockFlow.Application.Exceptions;
using StockFlow.Application.Interfaces;
using StockFlow.Application.Interfaces.IAudit;
using StockFlow.Application.Interfaces.Logistics;
using StockFlow.Application.Interfaces.Quality;
using StockFlow.Domain.Entities.Quality;
using StockFlow.Domain.Enums.Audit;
using System.Text.Json;

namespace StockFlow.Application.Services.Quality
{
    public class DeliveryIssueService : IDeliveryIssueService
    {
        private readonly IDeliveryIssueRepository _deliveryIssueRepository;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IAuditService _auditService;

        private readonly IValidator<CreateDeliveryIssueDto> _createValidator;
        private readonly IValidator<UpdateDeliveryIssueDto> _updateValidator;
        private readonly IValidator<ResolveDeliveryIssueDto> _resolveValidator;

        public DeliveryIssueService(
            IDeliveryIssueRepository deliveryIssueRepository,
            IDeliveryRepository deliveryRepository,
            IAuditService auditService,
            IValidator<CreateDeliveryIssueDto> createValidator,
            IValidator<UpdateDeliveryIssueDto> updateValidator,
            IValidator<ResolveDeliveryIssueDto> resolveValidator)
        {
            _deliveryIssueRepository = deliveryIssueRepository;
            _deliveryRepository = deliveryRepository;
            _auditService = auditService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _resolveValidator = resolveValidator;
        }

        public async Task<DeliveryIssueResponseDto> CreateAsync(CreateDeliveryIssueDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var delivery = await _deliveryRepository.GetByIdAsync(dto.DeliveryId);

            if (delivery is null)
            {
                throw new NotFoundException($"Delivery with ID {dto.DeliveryId} was not found.");
            }

            var issue = new DeliveryIssue
            {
                DeliveryId = dto.DeliveryId,
                IssueType = dto.IssueType,
                Description = dto.Description,
                OccurredAt = DateTime.UtcNow,
                IsResolved = false
            };

            await _deliveryIssueRepository.AddAsync(issue);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Create,
                Entity = AuditEntity.DeliveryIssue,
                EntityId = issue.Id.ToString(),
                NewValues = JsonSerializer.Serialize(issue),
                Success = true
            });

            return await GetByIdAsync(issue.Id);
        }

        public async Task<IEnumerable<DeliveryIssueResponseDto>> GetAllAsync()
        {
            var issues = await _deliveryIssueRepository.GetAllAsync();

            return issues.Select(issue => new DeliveryIssueResponseDto
            {
                Id = issue.Id,
                DeliveryId = issue.DeliveryId,
                IssueType = issue.IssueType,
                Description = issue.Description,
                IsResolved = issue.IsResolved,
                OccurredAt = issue.OccurredAt,
                ResolvedAt = issue.ResolvedAt,
                ResolutionNotes = issue.ResolutionNotes
            });
        }

        public async Task<IEnumerable<DeliveryIssueResponseDto>> GetByDeliveryAsync(int deliveryId)
        {
            if (deliveryId <= 0)
            {
                throw new BadRequestException("Invalid delivery ID.");
            }

            var delivery = await _deliveryRepository.GetByIdAsync(deliveryId);

            if (delivery is null)
            {
                throw new NotFoundException($"Delivery with ID {deliveryId} was not found.");
            }

            var issues = await _deliveryIssueRepository.GetByDeliveryIdAsync(deliveryId);

            if (!issues.Any())
            {
                throw new NotFoundException($"No delivery issues found for delivery ID {deliveryId}.");
            }

            return issues.Select(issue => new DeliveryIssueResponseDto
            {
                Id = issue.Id,
                DeliveryId = issue.DeliveryId,
                IssueType = issue.IssueType,
                Description = issue.Description,
                IsResolved = issue.IsResolved,
                OccurredAt = issue.OccurredAt,
                ResolvedAt = issue.ResolvedAt,
                ResolutionNotes = issue.ResolutionNotes
            });
        }

        public async Task<DeliveryIssueResponseDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid delivery issue ID.");
            }

            var issue = await _deliveryIssueRepository.GetByIdAsync(id);

            if (issue is null)
            {
                throw new NotFoundException($"Delivery issue with ID {id} was not found.");
            }

            return new DeliveryIssueResponseDto
            {
                Id = issue.Id,
                DeliveryId = issue.DeliveryId,
                IssueType = issue.IssueType,
                Description = issue.Description,
                IsResolved = issue.IsResolved,
                OccurredAt = issue.OccurredAt,
                ResolvedAt = issue.ResolvedAt,
                ResolutionNotes = issue.ResolutionNotes
            };
        }

        public async Task<IEnumerable<DeliveryIssueResponseDto>> GetOpenIssuesAsync()
        {
            var issues = await _deliveryIssueRepository.GetOpenIssuesAsync();

            if (!issues.Any())
            {
                throw new NotFoundException("No open delivery issues were found.");
            }

            return issues.Select(issue => new DeliveryIssueResponseDto
            {
                Id = issue.Id,
                DeliveryId = issue.DeliveryId,
                IssueType = issue.IssueType,
                Description = issue.Description,
                IsResolved = issue.IsResolved,
                OccurredAt = issue.OccurredAt,
                ResolvedAt = issue.ResolvedAt,
                ResolutionNotes = issue.ResolutionNotes
            });
        }

        public async Task<IEnumerable<DeliveryIssueResponseDto>> GetResolvedIssuesAsync()
        {
            var issues = await _deliveryIssueRepository.GetResolvedIssuesAsync();

            if (!issues.Any())
            {
                throw new NotFoundException("No resolved delivery issues were found.");
            }

            return issues.Select(issue => new DeliveryIssueResponseDto
            {
                Id = issue.Id,
                DeliveryId = issue.DeliveryId,
                IssueType = issue.IssueType,
                Description = issue.Description,
                IsResolved = issue.IsResolved,
                OccurredAt = issue.OccurredAt,
                ResolvedAt = issue.ResolvedAt,
                ResolutionNotes = issue.ResolutionNotes
            });
        }

        public async Task ResolveAsync(int issueId, ResolveDeliveryIssueDto dto)
        {
            if (issueId <= 0)
            {
                throw new BadRequestException("Invalid delivery issue ID.");
            }

            var validationResult = await _resolveValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(
                    validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var issue = await _deliveryIssueRepository.GetByIdAsync(issueId);

            if (issue is null)
            {
                throw new NotFoundException($"Delivery issue with ID {issueId} was not found.");
            }

            if (issue.IsResolved)
            {
                throw new BadRequestException("Delivery issue is already resolved.");
            }

            issue.IsResolved = true;
            issue.ResolvedAt = DateTime.UtcNow;
            issue.ResolutionNotes = dto.ResolutionNotes;

            await _deliveryIssueRepository.UpdateAsync(issue);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.DeliveryIssue,
                EntityId = issue.Id.ToString(),
                NewValues = JsonSerializer.Serialize(issue),
                Success = true
            });
        }

        public async Task<DeliveryIssueResponseDto> UpdateAsync(int id, UpdateDeliveryIssueDto dto)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid delivery issue ID.");
            }

            var validationResult = await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var issue = await _deliveryIssueRepository.GetByIdAsync(id);

            if (issue is null)
            {
                throw new NotFoundException($"Delivery issue with ID {id} was not found.");
            }

            issue.IssueType = dto.IssueType;
            issue.Description = dto.Description;

            await _deliveryIssueRepository.UpdateAsync(issue);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.DeliveryIssue,
                EntityId = issue.Id.ToString(),
                NewValues = JsonSerializer.Serialize(issue),
                Success = true
            });

            return new DeliveryIssueResponseDto
            {
                Id = issue.Id,
                DeliveryId = issue.DeliveryId,
                IssueType = issue.IssueType,
                Description = issue.Description,
                IsResolved = issue.IsResolved,
                OccurredAt = issue.OccurredAt,
                ResolvedAt = issue.ResolvedAt,
                ResolutionNotes = issue.ResolutionNotes
            };
        }
    }
}
