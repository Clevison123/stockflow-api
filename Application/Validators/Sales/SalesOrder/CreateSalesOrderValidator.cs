using FluentValidation;
using StockFlow.Application.DTOs.Sales.SalesOrder;

namespace StockFlow.Application.Validators.Sales.SalesOrder
{
    public class CreateSalesOrderValidator : AbstractValidator<CreateSalesOrderDto>
    {
        public CreateSalesOrderValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("Cliente inválido.");

            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .WithMessage("As observações devem possuir no máximo 1000 caracteres.");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("O pedido deve possuir pelo menos um item.");

            RuleForEach(x => x.Items)
                .NotNull()
                .WithMessage("Todos os itens do pedido são obrigatórios.");
        }
    }
}