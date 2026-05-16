using food_heaven_backend.PlanComidas.Domain.Model.Entities;
using food_heaven_backend.Shared.Domain.Repositories;

namespace food_heaven_backend.PlanComidas.Domain.Repositories;

public interface IPlanComidaRepository : IBaseRepository<PlanComida>
{
    Task<List<PlanComida>> FindPlanComidasByUserIdAsync(int id);

    Task<bool> ExistsOverlappingPlanForUserAsync(
        int userId,
        DateTime startDate,
        DateTime endDate,
        int? excludedPlanId = null);
}
