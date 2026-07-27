using FluentValidation;
using StockFlow.Application.DTOs.Sales.Customers;

namespace StockFlow.Application.Validators.Sales.Customers
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.TradeName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(150);

            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(200);

            RuleFor(x => x.Cnpj)
                .NotEmpty()
                .Length(14)
                .Matches(@"^\d+$")
                .WithMessage("O CNPJ deve conter apenas números.");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(20);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(150);

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(300);

            RuleFor(x => x.City)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.State)
                .NotEmpty()
                .Length(2)
                .WithMessage("Informe a sigla do estado.");

            RuleFor(x => x.PostalCode)
                .NotEmpty()
                .Length(8)
                .Matches(@"^\d+$")
                .WithMessage("O CEP deve conter apenas números.");
        }
    }
}