using FluentValidation;
using StockFlow.Application.DTOs.Logistics.Delivery;

namespace StockFlow.Application.Validators.Logistics.Delivery
{
    public class CreateDeliveryValidator
        : AbstractValidator<CreateDeliveryDto>
    {
        public CreateDeliveryValidator()
        {
            RuleFor(x => x.SalesOrderId)
                .GreaterThan(0)
                .WithMessage("Pedido inválido.");

            RuleFor(x => x.DriverName)
                .NotEmpty()
                .WithMessage("O motorista é obrigatório.")
                .Length(3, 100)
                .WithMessage("O nome do motorista deve possuir entre 3 e 100 caracteres.");

            RuleFor(x => x.VehiclePlate)
                .NotEmpty()
                .WithMessage("A placa do veículo é obrigatória.")
                .MaximumLength(20)
                .WithMessage("A placa deve possuir no máximo 20 caracteres.");

            RuleFor(x => x.DeliveryAddress)
                .NotEmpty()
                .WithMessage("O endereço de entrega é obrigatório.")
                .MaximumLength(300)
                .WithMessage("O endereço deve possuir no máximo 300 caracteres.");

            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .WithMessage("As observações devem possuir no máximo 1000 caracteres.");
        }
    }
}