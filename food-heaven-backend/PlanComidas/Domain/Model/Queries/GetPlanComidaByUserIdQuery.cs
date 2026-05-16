namespace food_heaven_backend.PlanComidas.Domain.Model.Queries;

public class GetPlanComidaByUserIdQuery
{
    public GetPlanComidaByUserIdQuery(int idUsuario)
    {
        IdUsuario = idUsuario;
    }

    public int IdUsuario { get; init; }
}
