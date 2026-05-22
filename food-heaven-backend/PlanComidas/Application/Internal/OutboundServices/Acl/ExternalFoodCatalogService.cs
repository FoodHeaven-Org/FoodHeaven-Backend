using food_heaven_backend.FoodCatalogContext.Interfaces.Acl;

namespace food_heaven_backend.PlanComidas.Application.Internal.OutboundServices.Acl;

public class ExternalFoodCatalogService(IFoodCatalogContextFacade foodCatalogContextFacade)
{
    private const int DaysInWeek = 7;
    private const int MealTypesPerDay = 3;

    private readonly IFoodCatalogContextFacade _foodCatalogContextFacade = foodCatalogContextFacade ?? throw new ArgumentNullException(nameof(foodCatalogContextFacade));

    public async Task EnsureMealsExistAsync(IEnumerable<int> mealIds)
    {
        ArgumentNullException.ThrowIfNull(mealIds);

        var selectedMealIds = mealIds
            .Where(id => id > 0)
            .Distinct()
            .ToArray();

        if (selectedMealIds.Length == 0) return;

        if (!await _foodCatalogContextFacade.MealIdsExistAsync(selectedMealIds))
        {
            throw new ArgumentException("The meal plan contains meals that do not exist in the food catalog.", nameof(mealIds));
        }
    }

    public async Task EnsureMealSlotsMatchMealTypesAsync(int[] mealSlots)
    {
        ArgumentNullException.ThrowIfNull(mealSlots);

        var selectedMealIds = mealSlots
            .Where(id => id > 0)
            .Distinct()
            .ToArray();

        if (selectedMealIds.Length == 0) return;

        var mealTypeIdsByMealId = await _foodCatalogContextFacade.GetMealTypeIdsByMealIdsAsync(selectedMealIds);

        for (var mealTypeIndex = 0; mealTypeIndex < MealTypesPerDay; mealTypeIndex++)
        {
            var expectedMealTypeId = mealTypeIndex + 1;

            for (var dayIndex = 0; dayIndex < DaysInWeek; dayIndex++)
            {
                var slotIndex = (mealTypeIndex * DaysInWeek) + dayIndex;
                var mealId = mealSlots[slotIndex];

                if (mealId <= 0) continue;

                if (!mealTypeIdsByMealId.TryGetValue(mealId, out var actualMealTypeId)
                    || actualMealTypeId != expectedMealTypeId)
                {
                    throw new InvalidOperationException("Meal slots must match their meal type: breakfast, lunch, or dinner.");
                }
            }
        }
    }
}
