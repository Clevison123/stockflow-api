using FluentValidation;
using StockFlow.Application.DTOs.Catalog.ProductVariant;

namespace StockFlow.Application.Validators.Catalog.ProductVariant
{
    public class CreateProductVariantValidator
        : AbstractValidator<CreateProductVariantDto>
    {
        public CreateProductVariantValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("Product is required.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Variant name is required.")
                .MinimumLength(2)
                .WithMessage("Variant name must contain at least 2 characters.")
                .MaximumLength(100)
                .WithMessage("Variant name cannot exceed 100 characters.");

            RuleFor(x => x.Color)
                .MaximumLength(50)
                .WithMessage("Color cannot exceed 50 characters.");

            RuleFor(x => x.Size)
                .MaximumLength(50)
                .WithMessage("Size cannot exceed 50 characters.");

            RuleFor(x => x.Storage)
                .MaximumLength(50)
                .WithMessage("Storage cannot exceed 50 characters.");

            RuleFor(x => x.Memory)
                .MaximumLength(50)
                .WithMessage("Memory cannot exceed 50 characters.");

            RuleFor(x => x.SalePrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Sale price cannot be negative.");
        }
    }
}