using FluentValidation;
using StockFlow.Application.DTOs.Sales.SalesOrderItem;

namespace StockFlow.Application.Validators.Sales.SalesOrderItem
{
    public class CreateSalesOrderItemValidator
        : AbstractValidator<CreateSalesOrderItemDto>
    {
        public CreateSalesOrderItemValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("Produto inválido.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("A quantidade deve ser maior que zero.");
        }
    }
}