using food_heaven_backend.FoodCatalogContext.Domain.Repositories;
using food_heaven_backend.FoodCatalogContext.Interfaces.Acl;

namespace food_heaven_backend.FoodCatalogContext.Application.Acl;

public class FoodCatalogContextFacadeImpl(IComidaRepository comidaRepository) : IFoodCatalogContextFacade
{
    private readonly IComidaRepository _comidaRepository = comidaRepository ?? throw new ArgumentNullException(nameof(comidaRepository));

    public Task<bool> MealIdsExistAsync(IEnumerable<int> mealIds)
    {
        ArgumentNullException.ThrowIfNull(mealIds);
        return _comidaRepository.AllMealIdsExistAsync(mealIds);
    }
}
