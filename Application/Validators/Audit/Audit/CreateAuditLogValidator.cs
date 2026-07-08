using FluentValidation;
using StockFlow.Application.DTOs.Audit;

namespace StockFlow.Application.Validators.Audit.Audit
{
    public class CreateAuditLogValidator
        : AbstractValidator<CreateAuditLogDto>
    {
        public CreateAuditLogValidator()
        {
            RuleFor(x => x.Action)
                .IsInEnum()
                .WithMessage("A ação informada é inválida.");

            RuleFor(x => x.Entity)
                .IsInEnum()
                .WithMessage("A entidade informada é inválida.");

            RuleFor(x => x.EntityId)
                .NotEmpty()
                .WithMessage("O identificador da entidade é obrigatório.")
                .MaximumLength(50)
                .WithMessage("O identificador da entidade deve possuir no máximo 50 caracteres.");

            RuleFor(x => x.OldValues)
                .MaximumLength(5000)
                .When(x => !string.IsNullOrWhiteSpace(x.OldValues));

            RuleFor(x => x.NewValues)
                .MaximumLength(5000)
                .When(x => !string.IsNullOrWhiteSpace(x.NewValues));
            
            RuleFor(x => x.ErrorMessage)
                .MaximumLength(2000)
                .When(x => !string.IsNullOrWhiteSpace(x.ErrorMessage));

            RuleFor(x => x.IpAddress)
                .MaximumLength(45)
                .When(x => !string.IsNullOrWhiteSpace(x.IpAddress));

            RuleFor(x => x.UserAgent)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.UserAgent));
        }
    }
}