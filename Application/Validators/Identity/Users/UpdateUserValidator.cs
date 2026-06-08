using FluentValidation;
using StockFlow.Application.DTOs.Identity.Users;

namespace StockFlow.Application.Validators.Identity.Users
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("O nome é obrigatório.")
                .MinimumLength(3)
                .WithMessage("O nome deve possuir no mínimo 3 caracteres.")
                .MaximumLength(100)
                .WithMessage("O nome deve possuir no máximo 100 caracteres.")
                .Matches(@"^[A-Za-zÀ-ÿ\s]+$")
                .WithMessage("O nome deve conter apenas letras e espaços.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O e-mail é obrigatório.")
                .EmailAddress()
                .WithMessage("Informe um e-mail válido.")
                .MaximumLength(150)
                .WithMessage("O e-mail deve possuir no máximo 150 caracteres.");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("O telefone é obrigatório.")
                .MinimumLength(10)
                .WithMessage("O telefone deve possuir no mínimo 10 caracteres.")
                .MaximumLength(20)
                .WithMessage("O telefone deve possuir no máximo 20 caracteres.");

            RuleFor(x => x.Role)
                .IsInEnum()
                .WithMessage("Cargo inválido.");
        }
    }
}