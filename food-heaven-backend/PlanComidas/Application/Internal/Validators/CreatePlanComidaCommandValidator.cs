using FluentValidation;
using food_heaven_backend.PlanComidas.Domain.Model.Commands;

namespace food_heaven_backend.PlanComidas.Application.Internal.Validators;

public class CreatePlanComidaCommandValidator : AbstractValidator<CreatePlanComidaCommand>
{
    public CreatePlanComidaCommandValidator()
    {
        RuleFor(p => p.IdUsuario)
            .GreaterThan(0).WithMessage("Invalid user ID.");

        RuleFor(p => p.FechaInicio)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(p => p.FechaFin)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(p => p.FechaInicio).WithMessage("End date must be after start date.");

        RuleFor(p => p.ListaComidas)
            .Must(array => array != null && array.Length == 21)
            .WithMessage("The weekly plan must contain exactly 21 meal slots.");

        RuleForEach(p => p.ListaComidas)
            .GreaterThan(0)
            .WithMessage("Each meal slot must reference a positive meal ID.");
    }
}
