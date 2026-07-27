using FluentValidation;
using StockFlow.Application.DTOs.Purchasing.InboundShipment;
using StockFlow.Application.Validators.Purchasing.InboundShipmentItem;

namespace StockFlow.Application.Validators.Purchasing.InboundShipment
{
    public class CreateInboundShipmentValidator
        : AbstractValidator<CreateInboundShipmentDto>
    {
        public CreateInboundShipmentValidator()
        {
            RuleFor(x => x.ShipmentNumber)
                .NotEmpty()
                .WithMessage("O número da remessa é obrigatório.")
                .MaximumLength(50)
                .WithMessage("O número da remessa deve possuir no máximo 50 caracteres.");

            RuleFor(x => x.ContainerNumber)
                .NotEmpty()
                .WithMessage("O número do container é obrigatório.")
                .MaximumLength(50)
                .WithMessage("O número do container deve possuir no máximo 50 caracteres.");

            RuleFor(x => x.OriginCountry)
                .NotEmpty()
                .WithMessage("O país de origem é obrigatório.")
                .MaximumLength(100)
                .WithMessage("O país de origem deve possuir no máximo 100 caracteres.");

            RuleFor(x => x.ArrivalDate)
                .NotEmpty()
                .WithMessage("A data prevista de chegada é obrigatória.");

            RuleFor(x => x.SupplierId)
                .GreaterThan(0)
                .WithMessage("Fornecedor inválido.");

            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .WithMessage("As observações devem possuir no máximo 1000 caracteres.");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("A remessa deve possuir pelo menos um item.");
           
            RuleForEach(x => x.Items)
                .SetValidator(new CreateInboundShipmentItemValidator());
        }
    }
}