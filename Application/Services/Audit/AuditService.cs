using FluentValidation;
using StockFlow.Application.DTOs.Audit;
using StockFlow.Application.Interfaces.IAudit;
using StockFlow.Domain.Entities.Audit;

namespace StockFlow.Application.Services.Audit
{
    public class AuditService : IAuditService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IValidator<CreateAuditLogDto> _validator;

        public AuditService(IAuditLogRepository auditLogRepository, IValidator<CreateAuditLogDto> validator)
        {
            _auditLogRepository = auditLogRepository;
            _validator = validator;
        }

        public async Task CreateAuditLogAsync(CreateAuditLogDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var auditLog = new AuditLog
            {
                Action = dto.Action,
                Entity = dto.Entity,
                EntityId = dto.EntityId ?? string.Empty,
                OldValues = dto.OldValues,
                NewValues = dto.NewValues,
                Success = dto.Success,
                ErrorMessage = dto.ErrorMessage,
                IpAddress = dto.IpAddress,
                UserAgent = dto.UserAgent
            };

            await _auditLogRepository.AddAsync(auditLog);


        }
    }
}
