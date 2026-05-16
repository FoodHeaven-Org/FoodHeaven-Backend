namespace food_heaven_backend.PlanComidas.Domain.Model.Queries;

public record GetPlanComidaByUserIdQuery
{
    public int IdUsuario { get; init; }

    public GetPlanComidaByUserIdQuery(int idUsuario)
    {
        if (idUsuario <= 0) throw new ArgumentException("IdUsuario must be greater than zero.", nameof(idUsuario));
        IdUsuario = idUsuario;
    }
}
