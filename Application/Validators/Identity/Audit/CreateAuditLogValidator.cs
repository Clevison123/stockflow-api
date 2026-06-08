using FluentValidation;
using StockFlow.Application.DTOs.Identity.Audit;

namespace StockFlow.Application.Validators.Identity.Audit
{
    public class CreateAuditLogValidator
        : AbstractValidator<CreateAuditLogDto>
    {
        public CreateAuditLogValidator()
        {
            RuleFor(x => x.Action)
                .NotEmpty()
                .WithMessage("A ação é obrigatória.")
                .MaximumLength(100)
                .WithMessage("A ação deve possuir no máximo 100 caracteres.");

            RuleFor(x => x.EntityName)
                .NotEmpty()
                .WithMessage("O nome da entidade é obrigatório.")
                .MaximumLength(100)
                .WithMessage("O nome da entidade deve possuir no máximo 100 caracteres.");

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
        }
    }
}