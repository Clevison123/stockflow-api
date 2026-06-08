using FluentValidation;
using StockFlow.Application.DTOs.Catalog.Product;

namespace StockFlow.Application.Validators.Catalog.Product
{
    public class CreateProductValidator
        : AbstractValidator<CreateProductDto>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O nome do produto é obrigatório.")
                .MinimumLength(3)
                .WithMessage("O nome do produto deve possuir no mínimo 3 caracteres.")
                .MaximumLength(150)
                .WithMessage("O nome do produto deve possuir no máximo 150 caracteres.");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("A descrição deve possuir no máximo 1000 caracteres.");

            RuleFor(x => x.SKU)
                .NotEmpty()
                .WithMessage("O SKU é obrigatório.")
                .MaximumLength(50)
                .WithMessage("O SKU deve possuir no máximo 50 caracteres.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("Categoria inválida.");

            RuleFor(x => x.SupplierId)
                .GreaterThan(0)
                .WithMessage("Fornecedor inválido.");

            RuleFor(x => x.OriginCountry)
                .NotEmpty()
                .WithMessage("O país de origem é obrigatório.")
                .MaximumLength(100)
                .WithMessage("O país de origem deve possuir no máximo 100 caracteres.");

            RuleFor(x => x.WarrantyMonths)
                .GreaterThanOrEqualTo(0)
                .WithMessage("A garantia não pode ser negativa.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0)
                .WithMessage("O preço deve ser maior que zero.");

            RuleFor(x => x.MinimumStock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("O estoque mínimo não pode ser negativo.");
        }
    }
}