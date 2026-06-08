using FluentValidation;
using StockFlow.Application.DTOs.SupplierClaim;

namespace StockFlow.Application.Validators.Quality.SupplierClaim
{
    public class UpdateSupplierClaimValidator
        : AbstractValidator<UpdateSupplierClaimDto>
    {
        public UpdateSupplierClaimValidator()
        {
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
        }
    }
}