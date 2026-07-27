using FluentValidation;
using StockFlow.Application.DTOs.Inventory.StockMovement;

public class CreateStockMovementValidator : AbstractValidator<CreateStockMovementDto>
{
    public CreateStockMovementValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("Produto inválido.");

        RuleFor(x => x.MovementType)
            .IsInEnum()
            .WithMessage("Tipo de movimentação inválido.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("A quantidade deve ser maior que zero.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("O motivo da movimentação é obrigatório.")
            .MinimumLength(5)
            .WithMessage("O motivo deve possuir no mínimo 5 caracteres.")
            .MaximumLength(500)
            .WithMessage("O motivo deve possuir no máximo 500 caracteres.");
    }
}