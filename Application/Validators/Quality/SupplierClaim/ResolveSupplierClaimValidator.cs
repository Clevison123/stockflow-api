using FluentValidation;
using StockFlow.Application.DTOs.SupplierClaim;

namespace StockFlow.Application.Validators.Quality.SupplierClaim
{
    public class ResolveSupplierClaimValidator
        : AbstractValidator<ResolveSupplierClaimDto>
    {
        public ResolveSupplierClaimValidator()
        {
            RuleFor(x => x.ResolutionNotes)
                .NotEmpty()
                .WithMessage("A descrição da resolução é obrigatória.")
                .MinimumLength(10)
                .WithMessage("A descrição da resolução deve possuir no mínimo 10 caracteres.")
                .MaximumLength(1000)
                .WithMessage("A descrição da resolução deve possuir no máximo 1000 caracteres.");
        }
    }
}