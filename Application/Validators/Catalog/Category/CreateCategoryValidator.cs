using FluentValidation;
using StockFlow.Application.DTOs.Catalog.Category;

namespace StockFlow.Application.Validators.Catalog.Category
{
    public class CreateCategoryValidator
        : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O nome da categoria é obrigatório.")
                .MinimumLength(3)
                .WithMessage("O nome da categoria deve possuir no mínimo 3 caracteres.")
                .MaximumLength(100)
                .WithMessage("O nome da categoria deve possuir no máximo 100 caracteres.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("A descrição deve possuir no máximo 500 caracteres.");
        }
    }
}