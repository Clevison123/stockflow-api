using FluentValidation;
using StockFlow.Application.DTOs.Quality.CustomerClaim;

namespace StockFlow.Application.Validators.Quality.CustomerClaim
{
    public class UpdateCustomerClaimValidator
        : AbstractValidator<UpdateCustomerClaimDto>
    {
        public UpdateCustomerClaimValidator()
        {
            RuleFor(x => x.ClaimType)
                .IsInEnum()
                .WithMessage("Tipo de reclamação inválido.");


            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("A descrição da reclamação é obrigatória.")
                .MinimumLength(10)
                .WithMessage("A descrição deve possuir no mínimo 10 caracteres.")
                .MaximumLength(1000)
                .WithMessage("A descrição deve possuir no máximo 1000 caracteres.");
        }
    }
}