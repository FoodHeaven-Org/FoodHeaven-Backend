using food_heaven_backend.PlanComidas.Domain.Model.Entities;
using food_heaven_backend.PlanComidas.Interfaces.Rest.Resources;

namespace food_heaven_backend.PlanComidas.Interfaces.Rest.Transform;

public static class PlanComidaResourceFromEntityAssembler
{
    public static PlanComidaResource ToResourceFromEntity(PlanComida plan)
    {
        return new PlanComidaResource(
            plan.Id,
            plan.IdUsuario,
            plan.FechaInicio,
            plan.FechaFin,
            plan.ListaComidas
        );
    }
    
    public static IEnumerable<PlanComidaResource> ToResourcesFromEntities(IEnumerable<PlanComida> entities)
    {
        return entities.Select(ToResourceFromEntity);
    }
}
