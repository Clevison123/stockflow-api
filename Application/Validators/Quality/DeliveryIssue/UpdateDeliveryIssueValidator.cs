using FluentValidation;
using StockFlow.Application.DTOs.Quality.DeliveryIssue;

namespace StockFlow.Application.Validators.Quality.DeliveryIssue
{
    public class UpdateDeliveryIssueValidator
        : AbstractValidator<UpdateDeliveryIssueDto>
    {
        public UpdateDeliveryIssueValidator()
        {
            RuleFor(x => x.IssueType)
                .IsInEnum()
                .WithMessage("Tipo de problema inválido.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("A descrição do problema é obrigatória.")
                .MinimumLength(10)
                .WithMessage("A descrição deve possuir no mínimo 10 caracteres.")
                .MaximumLength(1000)
                .WithMessage("A descrição deve possuir no máximo 1000 caracteres.");
        }
    }
}