using FluentValidation;
using StockFlow.API.src.Application.DTOs.StockMovement;

namespace StockFlow.API.src.Application.Validators
{
    public class CreateStockMovementDtoValidator : AbstractValidator<CreateStockMovementDto>
    {
        public CreateStockMovementDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("A valid product must be selected.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Reason is required.")
                .MaximumLength(200).WithMessage("Reason must have a maximum of 200 characters.");
        }
    }
}