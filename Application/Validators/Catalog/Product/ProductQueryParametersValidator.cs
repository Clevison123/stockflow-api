using FluentValidation;
using StockFlow.Application.DTOs.Catalog.Product;

namespace StockFlow.Application.Validators.Catalog.Product
{
    public class ProductQueryParametersValidator
        : AbstractValidator<ProductQueryParameters>
    {
        public ProductQueryParametersValidator()
        {
            RuleFor(x => x.Search)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Search));

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .When(x => x.CategoryId.HasValue);

            RuleFor(x => x.SupplierId)
                .GreaterThan(0)
                .When(x => x.SupplierId.HasValue);

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("O número da página deve ser maior que zero.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("O tamanho da página deve estar entre 1 e 100.");
        }
    }
}