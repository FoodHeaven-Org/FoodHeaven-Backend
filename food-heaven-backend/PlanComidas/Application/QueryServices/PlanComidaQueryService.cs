using food_heaven_backend.PlanComidas.Domain.Model.Entities;
using food_heaven_backend.PlanComidas.Domain.Model.Queries;
using food_heaven_backend.PlanComidas.Domain.Repositories;
using food_heaven_backend.PlanComidas.Domain.Services;

namespace food_heaven_backend.PlanComidas.Application.QueryServices;

public class PlanComidaQueryService(IPlanComidaRepository repository) : IPlanComidaQueryService
{
    private readonly IPlanComidaRepository _repository = repository;

    public async Task<IEnumerable<PlanComida>> Handle(GetAllPlanComidasQuery query)
    {
        var items = await _repository.ListAsync();
        return items ?? Enumerable.Empty<PlanComida>();
    }

    public async Task<IEnumerable<PlanComida>> Handle(GetPlanComidaByUserIdQuery query)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));
        var planes = await _repository.FindPlanComidasByUserIdAsync(query.IdUsuario);
        return planes ?? Enumerable.Empty<PlanComida>();
    }



}
