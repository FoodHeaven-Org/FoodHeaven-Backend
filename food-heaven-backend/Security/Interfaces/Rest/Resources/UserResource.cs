using food_heaven_backend.Security.Domain.Model.Entities;
using food_heaven_backend.Security.Domain.Model.ValueObjects;
using System.Text.Json;

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
    IReadOnlyCollection<DeliveryAddressResource> DeliveryAddresses,
    string PaymentMethod,
    PaymentCardResource PaymentCard
)
{
    public static UserResource FromEntity(User user)
    {
        var plan = UserSubscriptionPlan.FromCode(user.Subscription);
        var paymentCard = PaymentCardResource.FromEntity(user);
        var deliveryAddresses = ReadDeliveryAddresses(user);

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
            deliveryAddresses,
            paymentCard.DisplayName,
            paymentCard
        );
    }

    private static IReadOnlyCollection<DeliveryAddressResource> ReadDeliveryAddresses(User user)
    {
        var addresses = string.IsNullOrWhiteSpace(user.DeliveryAddressesJson)
            ? []
            : JsonSerializer.Deserialize<List<DeliveryAddress>>(user.DeliveryAddressesJson, new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? [];

        if (addresses.Count == 0 && !string.IsNullOrWhiteSpace(user.Address))
        {
            addresses.Add(new DeliveryAddress("Principal", user.Address, isDefault: true));
        }

        return addresses.Select(DeliveryAddressResource.FromValueObject).ToList();
    }
}

public record DeliveryAddressResource(
    string Label,
    string AddressLine,
    double? Latitude,
    double? Longitude,
    bool IsDefault
)
{
    public static DeliveryAddressResource FromValueObject(DeliveryAddress address)
    {
        return new DeliveryAddressResource(
            address.Label,
            address.AddressLine,
            address.Latitude,
            address.Longitude,
            address.IsDefault
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
