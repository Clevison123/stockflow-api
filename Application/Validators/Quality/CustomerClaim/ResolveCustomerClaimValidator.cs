using FluentValidation;
using StockFlow.Application.DTOs.Quality.CustomerClaim;

namespace StockFlow.Application.Validators.Quality.CustomerClaim
{
    public class ResolveCustomerClaimValidator
        : AbstractValidator<ResolveCustomerClaimDto>
    {
        public ResolveCustomerClaimValidator()
        {
            RuleFor(x => x.ResolutionNotes)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("A descrição da resolução é obrigatória.")
                .MinimumLength(10)
                .WithMessage("A descrição da resolução deve possuir no mínimo 10 caracteres.")
                .MaximumLength(1000)
                .WithMessage("A descrição da resolução deve possuir no máximo 1000 caracteres.");
        }
    }
}