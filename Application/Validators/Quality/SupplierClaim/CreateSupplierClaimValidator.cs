using FluentValidation;
using StockFlow.Application.DTOs.SupplierClaim;

namespace StockFlow.Application.Validators.Quality.SupplierClaim
{
    public class CreateSupplierClaimValidator
        : AbstractValidator<CreateSupplierClaimDto>
    {
        public CreateSupplierClaimValidator()
        {
            RuleFor(x => x.SupplierId)
                .GreaterThan(0)
                .WithMessage("Fornecedor inválido.");

            RuleFor(x => x.ClaimType)
                .IsInEnum()
                .WithMessage("Tipo de reclamação inválido.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("A descrição da reclamação é obrigatória.")
                .MinimumLength(10)
                .WithMessage("A descrição deve possuir no mínimo 10 caracteres.")
                .MaximumLength(1000)
                .WithMessage("A descrição deve possuir no máximo 1000 caracteres.");

            RuleFor(x => x.QualityIssueId)
                .GreaterThan(0)
                .When(x => x.QualityIssueId.HasValue)
                .WithMessage("Problema de qualidade inválido.");
        }
    }
}