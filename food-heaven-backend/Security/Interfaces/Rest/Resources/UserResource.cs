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
    string PaymentMethod,
    PaymentCardResource PaymentCard
)
{
    public static UserResource FromEntity(User user)
    {
        var plan = UserSubscriptionPlan.FromCode(user.Subscription);
        var paymentCard = PaymentCardResource.FromEntity(user);

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
            paymentCard.DisplayName,
            paymentCard
        );
    }
}

public record PaymentCardResource(
    string Brand,
    string LastFour,
    string Expiration,
    string DisplayName
)
{
    public static PaymentCardResource FromEntity(User user)
    {
        var brand = user.PaymentCardBrand;
        var lastFour = user.PaymentCardLastFour;
        var expiration = user.PaymentCardExpiration;
        var displayName = string.IsNullOrWhiteSpace(lastFour)
            ? user.PaymentMethod
            : $"{brand} ending in {lastFour}";

        return new PaymentCardResource(brand, lastFour, expiration, displayName);
    }
}
