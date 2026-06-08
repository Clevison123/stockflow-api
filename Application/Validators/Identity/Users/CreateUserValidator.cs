using FluentValidation;
using StockFlow.Application.DTOs.Identity.Users;

namespace StockFlow.Application.Validators.Identity.Users
{
    public class CreateUserValidator
        : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
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

            RuleFor(x => x.EmployeeCode)
                .NotEmpty()
                .WithMessage("O código do funcionário é obrigatório.")
                .MaximumLength(30)
                .WithMessage("O código do funcionário deve possuir no máximo 30 caracteres.");

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
                .Matches("[^a-zA-Z0-9]")
                .WithMessage("A senha deve conter pelo menos um caractere especial.");

            RuleFor(x => x.Role)
                .IsInEnum()
                .WithMessage("Cargo inválido.");
        }
    }
}