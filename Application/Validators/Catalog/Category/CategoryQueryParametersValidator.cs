using FluentValidation;
using StockFlow.Application.DTOs.Catalog.Category;

namespace StockFlow.Application.Validators.Catalog.Category
{
    public class CategoryQueryParametersValidator
        : AbstractValidator<CategoryQueryParametersDto>
    {
        public CategoryQueryParametersValidator()
        {
            RuleFor(x => x.Search)
                .MaximumLength(100)
                .WithMessage("O termo de pesquisa deve possuir no máximo 100 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Search));

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("O número da página deve ser maior que zero.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("O tamanho da página deve ser maior que zero.")
                .LessThanOrEqualTo(100)
                .WithMessage("O tamanho da página deve ser no máximo 100.");
        }
    }
}