using FluentValidation;
using StockFlow.Application.DTOs.Identity.Users;

namespace StockFlow.Application.Validators.Identity.Users
{
    public class ResetPasswordValidator
        : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("A nova senha é obrigatória.")
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
                .Matches("[^a-zA-Z0-9]")
                .WithMessage("A senha deve conter pelo menos um caractere especial.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage("A confirmação da senha é obrigatória.")
                .Equal(x => x.NewPassword)
                .WithMessage("As senhas não coincidem.");
        }
    }
}