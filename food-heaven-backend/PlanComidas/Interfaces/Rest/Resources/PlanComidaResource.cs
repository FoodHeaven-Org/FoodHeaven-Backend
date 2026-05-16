using food_heaven_backend.PlanComidas.Domain.Model.Entities;

namespace food_heaven_backend.PlanComidas.Interfaces.Rest.Resources;

public record PlanComidaResource(
    int Id,
    int IdUsuario,
    DateTime FechaInicio,
    DateTime FechaFin,
    int[] ListaComidas
)
{
    public static PlanComidaResource FromEntity(PlanComida plan)
    {
        return new PlanComidaResource(
            plan.Id,
            plan.IdUsuario,
            plan.FechaInicio,
            plan.FechaFin,
            plan.ListaComidas
        );
    }
}
