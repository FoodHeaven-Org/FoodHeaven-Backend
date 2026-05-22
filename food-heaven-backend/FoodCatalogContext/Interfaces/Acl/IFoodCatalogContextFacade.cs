namespace food_heaven_backend.FoodCatalogContext.Interfaces.Acl;

public interface IFoodCatalogContextFacade
{
    Task<bool> MealIdsExistAsync(IEnumerable<int> mealIds);

    Task<IReadOnlyDictionary<int, int>> GetMealTypeIdsByMealIdsAsync(IEnumerable<int> mealIds);
}
