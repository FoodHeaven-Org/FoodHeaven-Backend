namespace food_heaven_backend.Security.Interfaces.Acl;

public interface ISecurityContextFacade
{
    Task<int> GetMealsPerDayLimitByUserIdAsync(int userId);
}
