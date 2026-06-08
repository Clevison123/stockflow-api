using FluentValidation;
using StockFlow.Application.DTOs.Identity.Auth;

namespace StockFlow.Application.Validators.Identity.Auth
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
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

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("A senha é obrigatória.")
                .MinimumLength(8)
                .WithMessage("A senha deve possuir no mínimo 8 caracteres.")
                .MaximumLength(100)
                .WithMessage("A senha deve possuir no máximo 100 caracteres.")
                .Matches("[A-Z]")
                .WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
                .Matches("[a-z]")
                .WithMessage("A senha deve conter pelo menos uma letra minúscula.")
                .Matches("[0-9]")
                .WithMessage("A senha deve conter pelo menos um número.")
                .Matches(@"[^a-zA-Z0-9]")
                .WithMessage("A senha deve conter pelo menos um caractere especial.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Confirme sua senha.")
                .Equal(x => x.Password)
                .WithMessage("As senhas não coincidem.");
        }
    }
}