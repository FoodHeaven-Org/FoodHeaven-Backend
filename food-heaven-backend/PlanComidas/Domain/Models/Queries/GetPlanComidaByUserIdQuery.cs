namespace food_heaven_backend.PlanComidas.Domain.Models.Queries;

public class GetPlanComidaByUserIdQuery
{
    public GetPlanComidaByUserIdQuery(int idUsuario)
    {
        IdUsuario = idUsuario;
    }

    public int IdUsuario { get; init; }
}
