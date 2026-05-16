using food_heaven_backend.PlanComidas.Domain.Model.Entities;
using food_heaven_backend.PlanComidas.Domain.Repositories;
using food_heaven_backend.Shared.Infrastructure.Persistence.EfCore.Configuration;
using food_heaven_backend.Shared.Infrastructure.Persistence.EfCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace food_heaven_backend.PlanComidas.Infrastructure.Persistence.EfCore.Repositories;

public class PlanComidaRepository(FoodHeavenContext context)
    : BaseRepository<PlanComida>(context), IPlanComidaRepository
{
    public async Task<List<PlanComida>> FindPlanComidasByUserIdAsync(int id)
    {
        return await Context.Set<PlanComida>()
            .Where(p => p.IdUsuario == id)
            .OrderByDescending(p => p.FechaInicio)
            .ToListAsync();
    }

    public async Task<bool> ExistsOverlappingPlanForUserAsync(
        int userId,
        DateTime startDate,
        DateTime endDate,
        int? excludedPlanId = null)
    {
        var query = Context.Set<PlanComida>()
            .Where(p => p.IdUsuario == userId)
            .Where(p => p.FechaInicio < endDate && startDate < p.FechaFin);

        if (excludedPlanId.HasValue)
        {
            query = query.Where(p => p.Id != excludedPlanId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task RemoveByUserIdAsync(int userId)
    {
        var plans = await Context.Set<PlanComida>()
            .Where(plan => plan.IdUsuario == userId)
            .ToListAsync();

        Context.Set<PlanComida>().RemoveRange(plans);
    }
}
