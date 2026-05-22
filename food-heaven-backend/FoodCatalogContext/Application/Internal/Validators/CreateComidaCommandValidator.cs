using FluentValidation;
using food_heaven_backend.FoodCatalogContext.Domain.Model.Commands;

namespace food_heaven_backend.FoodCatalogContext.Application.Internal.Validators;

public class CreateComidaCommandValidator : AbstractValidator<CreateComidaCommand>
{
    public CreateComidaCommandValidator()
    {
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("Nombre is required")
            .MaximumLength(50).WithMessage("Nombre must be at most 50 characters");

        RuleFor(p => p.Complemento)
            .NotEmpty().WithMessage("Complemento is required")
            .MaximumLength(50).WithMessage("Complemento must be at most 50 characters");

        RuleFor(p => p.Url)
            .NotEmpty().WithMessage("Url is required")
            .Must(BeHttpUrl).WithMessage("Url must be an absolute HTTP or HTTPS URL");

        RuleFor(p => p.id_tipo_comida)
            .InclusiveBetween(1, 3).WithMessage("id_tipo_comida must be 1 (breakfast), 2 (lunch), or 3 (dinner)");

        RuleFor(p => p.Calorias)
            .GreaterThan(0).WithMessage("Calorias must be greater than zero");

        RuleFor(p => p.Proteina)
            .GreaterThanOrEqualTo(0).WithMessage("Proteina cannot be negative");

        RuleFor(p => p.Carbohidrato)
            .GreaterThanOrEqualTo(0).WithMessage("Carbohidrato cannot be negative");

        RuleFor(p => p.Grasa)
            .GreaterThanOrEqualTo(0).WithMessage("Grasa cannot be negative");
    }

    private static bool BeHttpUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}
