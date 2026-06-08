using FluentValidation;
using StockFlow.Application.DTOs.Sales.SalesOrder;

namespace StockFlow.Application.Validators.Sales.SalesOrder
{
    public class UpdateSalesOrderValidator
        : AbstractValidator<UpdateSalesOrderDto>
    {
        public UpdateSalesOrderValidator()
        {
            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .WithMessage("As observações devem possuir no máximo 1000 caracteres.");
        }
    }
}