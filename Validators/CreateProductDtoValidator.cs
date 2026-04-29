using FluentValidation;
using StockFlow.API.DTOs.Product;

namespace StockFlow.API.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must have a maximum of 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Product description is required.")
                .MaximumLength(500).WithMessage("Product description must have a maximum of 500 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.QuantityInStock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");

            RuleFor(x => x.MinimumStock)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum stock cannot be negative.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("A valid category must be selected.");

            RuleFor(x => x.SupplierId)
                .GreaterThan(0).WithMessage("A valid supplier must be selected.");
        }
    }
}