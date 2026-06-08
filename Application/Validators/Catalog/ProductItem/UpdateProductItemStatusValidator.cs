using FluentValidation;
using StockFlow.Application.DTOs.Catalog.ProductItem;

namespace StockFlow.Application.Validators.Catalog.ProductItem
{
    public class UpdateProductItemStatusValidator
        : AbstractValidator<UpdateProductItemStatusDto>
    {
        public UpdateProductItemStatusValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Status inválido.");
        }
    }
}