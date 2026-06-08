using FluentValidation;
using StockFlow.Application.DTOs.Quality.CustomerClaim;

namespace StockFlow.Application.Validators.Quality.CustomerClaim
{
    public class CreateCustomerClaimValidator
        : AbstractValidator<CreateCustomerClaimDto>
    {
        public CreateCustomerClaimValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("Cliente inválido.");

            RuleFor(x => x.SalesOrderId)
                .GreaterThan(0)
                .WithMessage("Pedido inválido.");

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