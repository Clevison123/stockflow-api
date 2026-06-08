using FluentValidation;
using StockFlow.Application.DTOs.Identity.Auth;

namespace StockFlow.Application.Validators.Identity.Auth
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O e-mail é obrigatório.")
                .EmailAddress()
                .WithMessage("Informe um e-mail válido.")
                .MaximumLength(150)
                .WithMessage("O e-mail deve possuir no máximo 150 caracteres.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("A senha é obrigatória.")
                .MinimumLength(8)
                .WithMessage("A senha deve possuir no mínimo 8 caracteres.")
                .MaximumLength(100)
                .WithMessage("A senha deve possuir no máximo 100 caracteres.");
        }
    }
}