using FluentValidation;
using StockFlow.API.DTOs.Auth;

namespace StockFlow.API.Application.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MinimumLength(3).WithMessage("Full name must have at least 3 characters.")
                .MaximumLength(100).WithMessage("Full name must have a maximum of 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must have at least 6 characters.")
                .MaximumLength(100).WithMessage("Password must have a maximum of 100 characters.");
        }
    }
}