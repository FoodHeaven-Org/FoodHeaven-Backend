using food_heaven_backend.FoodCatalogContext.Interfaces.Acl;

namespace food_heaven_backend.PlanComidas.Application.Internal.OutboundServices.Acl;

public class ExternalFoodCatalogService(IFoodCatalogContextFacade foodCatalogContextFacade)
{
    private readonly IFoodCatalogContextFacade _foodCatalogContextFacade = foodCatalogContextFacade ?? throw new ArgumentNullException(nameof(foodCatalogContextFacade));

    public async Task EnsureMealsExistAsync(IEnumerable<int> mealIds)
    {
        ArgumentNullException.ThrowIfNull(mealIds);

        if (!await _foodCatalogContextFacade.MealIdsExistAsync(mealIds))
        {
            throw new ArgumentException("The meal plan contains meals that do not exist in the food catalog.", nameof(mealIds));
        }
    }
}
