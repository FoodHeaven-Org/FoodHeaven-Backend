using FluentValidation;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Commands;

namespace food_heaven_backend.FoodCatalogContext.Domain.Models.Validators;

public class CreateProveedorCommandValidator : AbstractValidator<CreateProveedorCommand>
{
    public CreateProveedorCommandValidator()
    {
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("Nombre is required")
            .MaximumLength(50).WithMessage("Nombre must be at most 50 characters");

        RuleFor(p => p.Distrito)
            .NotEmpty().WithMessage("Distrito is required")
            .MaximumLength(50).WithMessage("Distrito must be at most 50 characters");

        RuleFor(p => p.Contacto)
            .NotEmpty().WithMessage("Contacto is required")
            .MaximumLength(50).WithMessage("Contacto must be at most 50 characters");

        RuleFor(p => p.TipoProveedorId)
            .GreaterThan(0).WithMessage("TipoProveedorId must be greater than 0");
    }
}