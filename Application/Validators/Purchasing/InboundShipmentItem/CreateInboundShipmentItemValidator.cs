using FluentValidation;
using StockFlow.Application.DTOs.InboundShipmentItem;

namespace StockFlow.Application.Validators.Purchasing.InboundShipmentItem
{
    public class CreateInboundShipmentItemValidator
        : AbstractValidator<CreateInboundShipmentItemDto>
    {
        public CreateInboundShipmentItemValidator()
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