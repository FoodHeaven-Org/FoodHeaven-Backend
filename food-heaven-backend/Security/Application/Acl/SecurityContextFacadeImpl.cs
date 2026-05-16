using food_heaven_backend.Security.Domain.Model.Queries;
using food_heaven_backend.Security.Domain.Model.ValueObjects;
using food_heaven_backend.Security.Domain.Services;
using food_heaven_backend.Security.Interfaces.Acl;

namespace food_heaven_backend.Security.Application.Acl;

public class SecurityContextFacadeImpl(IUserQueryService userQueryService) : ISecurityContextFacade
{
    public async Task<int> GetMealsPerDayLimitByUserIdAsync(int userId)
    {
        var user = await userQueryService.Handle(new GetUserByIdQuery(userId));
        return UserSubscriptionPlan.FromCode(user.Subscription).MealsPerDay;
    }
}
