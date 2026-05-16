using FluentValidation;
using food_heaven_backend.PlanComidas.Domain.Models.Commands;

namespace food_heaven_backend.PlanComidas.Domain.Models.Validators;

public class CreatePlanComidaCommandValidator : AbstractValidator<CreatePlanComidaCommand>
{
    public CreatePlanComidaCommandValidator()
    {
        RuleFor(p => p.IdUsuario)
            .GreaterThan(0).WithMessage("ID de usuario inválido.");

        RuleFor(p => p.FechaInicio)
            .NotEmpty().WithMessage("La fecha de inicio es obligatoria.");

        RuleFor(p => p.FechaFin)
            .NotEmpty().WithMessage("La fecha de fin es obligatoria.")
            .GreaterThan(p => p.FechaInicio).WithMessage("La fecha de fin debe ser posterior a la de inicio.");
        RuleFor(p => p.ListaComidas)
            .Must(array => array != null && array.Length == 21)
            .WithMessage("La lista debe contener exactamente 21 elementos.");
    }
}
