using FluentValidation;
using StockFlow.Application.DTOs.Purchasing.InboundShipment;

namespace StockFlow.Application.Validators.Purchasing.InboundShipment
{
    public class UpdateInboundShipmentValidator
        : AbstractValidator<UpdateInboundShipmentDto>
    {
        public UpdateInboundShipmentValidator()
        {
            RuleFor(x => x.ArrivalDate)
                .NotEmpty()
                .WithMessage("A data de chegada é obrigatória.");

            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .WithMessage("As observações devem possuir no máximo 1000 caracteres.");
        }
    }
}