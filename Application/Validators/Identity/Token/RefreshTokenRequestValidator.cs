using FluentValidation;
using StockFlow.Application.DTOs.Identity.Token;

namespace StockFlow.Application.Validators.Identity.Token
{
    public class RefreshTokenRequestValidator
        : AbstractValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithMessage("O refresh token é obrigatório.")
                .MinimumLength(20)
                .WithMessage("O refresh token informado é inválido.")
                .MaximumLength(500)
                .WithMessage("O refresh token informado é inválido.");
        }
    }
}