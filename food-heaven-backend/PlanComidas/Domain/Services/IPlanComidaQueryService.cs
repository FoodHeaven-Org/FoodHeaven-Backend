using food_heaven_backend.PlanComidas.Domain.Models.Entities;
using food_heaven_backend.PlanComidas.Domain.Models.Queries;

namespace food_heaven_backend.PlanComidas.Domain.Services;

public interface IPlanComidaQueryService
{
    Task<IEnumerable<PlanComida>> Handle(GetAllPlanComidasQuery query);
    Task<IEnumerable<PlanComida>> Handle(GetPlanComidaByUserIdQuery query);
}
