using FluentValidation;
using StockFlow.Application.DTOs.Sales.SalesOrder;

namespace StockFlow.Application.Validators.Sales.SalesOrder
{
    public class UpdateSalesOrderStatusValidator : AbstractValidator<UpdateSalesOrderStatusDto>
    {
        public UpdateSalesOrderStatusValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid sales order status.");
        }
    }
}
