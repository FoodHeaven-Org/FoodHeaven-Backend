using food_heaven_backend.Security.Interfaces.Acl;

namespace food_heaven_backend.PlanComidas.Application.Internal.OutboundServices.Acl;

public class ExternalUserSubscriptionService(ISecurityContextFacade securityContextFacade)
{
    private const int DaysInWeek = 7;
    private const int MealTypesPerDay = 3;

    public async Task EnsurePlanFitsSubscriptionAsync(int userId, int[] mealSlots)
    {
        var mealsPerDayLimit = await securityContextFacade.GetMealsPerDayLimitByUserIdAsync(userId);

        for (var dayIndex = 0; dayIndex < DaysInWeek; dayIndex++)
        {
            var selectedMeals = 0;

            for (var mealTypeIndex = 0; mealTypeIndex < MealTypesPerDay; mealTypeIndex++)
            {
                var slotIndex = (mealTypeIndex * DaysInWeek) + dayIndex;
                if (mealSlots[slotIndex] > 0)
                {
                    selectedMeals++;
                }
            }

            if (selectedMeals > mealsPerDayLimit)
            {
                throw new InvalidOperationException($"Your current subscription allows up to {mealsPerDayLimit} meal(s) per day.");
            }
        }
    }
}
