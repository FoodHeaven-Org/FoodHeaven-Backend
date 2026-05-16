using food_heaven_backend.Security.Domain.Model.Entities;
using food_heaven_backend.Security.Domain.Model.ValueObjects;

namespace food_heaven_backend.Security.Interfaces.Rest.Resources;

public record UserResource(
    int Id,
    string FullName,
    string Username,
    string Subscription,
    string SubscriptionName,
    int MealsPerDay,
    decimal MonthlyPrice,
    int Phone,
    string City,
    string Address,
    string PaymentMethod
)
{
    public static UserResource FromEntity(User user)
    {
        var plan = UserSubscriptionPlan.FromCode(user.Subscription);

        return new UserResource(
            user.Id,
            user.FullName,
            user.Username,
            plan.Code,
            plan.DisplayName,
            plan.MealsPerDay,
            plan.MonthlyPrice,
            user.Phone,
            user.City,
            user.Address,
            user.PaymentMethod
        );
    }
}
