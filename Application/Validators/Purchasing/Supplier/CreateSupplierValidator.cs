using FluentValidation;
using StockFlow.Application.DTOs.Purchasing.Supplier;

namespace StockFlow.Application.Validators.Purchasing.Supplier
{
    public class CreateSupplierValidator
        : AbstractValidator<CreateSupplierDto>
    {
        public CreateSupplierValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(150);

            RuleFor(x => x.ContactPerson)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(150);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(20);

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(300);

            RuleFor(x => x.Website)
                .MaximumLength(200)
                .Must(url =>
                    string.IsNullOrWhiteSpace(url) ||
                    Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("Website inválido.");
        }
    }
}