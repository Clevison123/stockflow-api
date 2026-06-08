using FluentValidation;
using StockFlow.Application.DTOs.Quality.QualityIssue;

namespace StockFlow.Application.Validators.Quality.QualityIssue
{
    public class CreateQualityIssueValidator
        : AbstractValidator<CreateQualityIssueDto>
    {
        public CreateQualityIssueValidator()
        {
            RuleFor(x => x.ProductItemId)
                .GreaterThan(0)
                .WithMessage("Item de produto inválido.");

            RuleFor(x => x.DetectedByUserId)
                .GreaterThan(0)
                .WithMessage("Usuário responsável inválido.");

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