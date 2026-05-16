using food_heaven_backend.PlanComidas.Domain.Models.Entities;
using food_heaven_backend.Shared.Domain.Repositories;

namespace food_heaven_backend.PlanComidas.Domain.Services;

public interface IPlanComidaRepository : IBaseRepository<PlanComida>
{
    Task<List<PlanComida>> FindPlanComidasByUserIdAsync(int id);
    
}
