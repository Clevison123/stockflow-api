using FluentValidation;
using StockFlow.Application.DTOs.Purchasing.Supplier;

namespace StockFlow.Application.Validators.Purchasing.Supplier
{
    public class UpdateSupplierValidator : AbstractValidator<UpdateSupplierDto>
    {
        public UpdateSupplierValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O nome do fornecedor é obrigatório.")
                .Length(3, 150)
                .WithMessage("O nome do fornecedor deve possuir entre 3 e 150 caracteres.");

            RuleFor(x => x.ContactPerson)
                .NotEmpty()
                .WithMessage("O responsável pelo contato é obrigatório.")
                .Length(3, 100)
                .WithMessage("O responsável deve possuir entre 3 e 100 caracteres.");

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
                .Length(10, 20)
                .WithMessage("O telefone deve possuir entre 10 e 20 caracteres.");

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