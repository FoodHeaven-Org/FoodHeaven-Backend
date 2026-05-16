using food_heaven_backend.PlanComidas.Domain.Model.Entities;
using food_heaven_backend.PlanComidas.Domain.Services;
using food_heaven_backend.Shared.Infrastructure.Persistence.Configuration;
using food_heaven_backend.Shared.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace food_heaven_backend.PlanComidas.Infrastructure;

public class PlanComidaRepository(FoodHeavenContext context)
    : BaseRepository<PlanComida>(context), IPlanComidaRepository
{
    public async Task<List<PlanComida>> FindPlanComidasByUserIdAsync(int id)
    {
        return await Context.Set<PlanComida>()
            .Where(p => p.IdUsuario == id)
            .ToListAsync();
    }

}
