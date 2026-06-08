using FluentValidation;
using StockFlow.Application.DTOs.Identity.Users;

namespace StockFlow.Application.Validators.Identity.Users
{
    public class ChangeUserRoleValidator : AbstractValidator<ChangeUserRoleDto>
    {
        public ChangeUserRoleValidator()
        {
            RuleFor(x => x.Role)
                .IsInEnum();

            RuleFor(x => x.Reason)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(300);
        }
    }
}
