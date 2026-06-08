using FluentValidation;
using StockFlow.Application.DTOs.Purchasing.Supplier;

namespace StockFlow.Application.Validators.Purchasing.Supplier
{
    public class UpdateSupplierValidator
        : AbstractValidator<UpdateSupplierDto>
    {
        public UpdateSupplierValidator()
        {
            RuleFor(x => x.ContactPerson)
                .NotEmpty()
                .WithMessage("O responsável pelo contato é obrigatório.")
                .MinimumLength(3)
                .WithMessage("O responsável deve possuir no mínimo 3 caracteres.")
                .MaximumLength(100)
                .WithMessage("O responsável deve possuir no máximo 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O e-mail é obrigatório.")
                .EmailAddress()
                .WithMessage("E-mail inválido.")
                .MaximumLength(150)
                .WithMessage("O e-mail deve possuir no máximo 150 caracteres.");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("O telefone é obrigatório.")
                .MinimumLength(10)
                .WithMessage("O telefone deve possuir no mínimo 10 caracteres.")
                .MaximumLength(20)
                .WithMessage("O telefone deve possuir no máximo 20 caracteres.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("O endereço é obrigatório.")
                .MaximumLength(300)
                .WithMessage("O endereço deve possuir no máximo 300 caracteres.");

            RuleFor(x => x.Website)
                .MaximumLength(200)
                .WithMessage("O website deve possuir no máximo 200 caracteres.")
                .Must(url =>
                    string.IsNullOrWhiteSpace(url) ||
                    Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("Website inválido.");
        }
    }
}