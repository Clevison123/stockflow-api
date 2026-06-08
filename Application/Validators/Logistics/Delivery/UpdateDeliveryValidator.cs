using FluentValidation;
using StockFlow.Application.DTOs.Logistics.Delivery;

namespace StockFlow.Application.Validators.Logistics.Delivery
{
    public class UpdateDeliveryValidator
        : AbstractValidator<UpdateDeliveryDto>
    {
        public UpdateDeliveryValidator()
        {
            RuleFor(x => x.DriverName)
                .NotEmpty()
                .WithMessage("O motorista é obrigatório.")
                .MinimumLength(3)
                .WithMessage("O nome do motorista deve possuir no mínimo 3 caracteres.")
                .MaximumLength(100)
                .WithMessage("O nome do motorista deve possuir no máximo 100 caracteres.");

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