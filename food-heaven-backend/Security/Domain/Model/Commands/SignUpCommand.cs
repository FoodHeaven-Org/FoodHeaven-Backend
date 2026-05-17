namespace food_heaven_backend.Security.Domain.Model.Commands;

using food_heaven_backend.Security.Domain.Model.ValueObjects;
using PaymentCardValueObject = food_heaven_backend.Security.Domain.Model.ValueObjects.PaymentCard;

public record SignUpCommand
{
    public string FullName { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
    public string Subscription { get; init; }
    public int Phone { get; init; }
    public string City { get; init; }
    public string Address { get; init; }
    public IReadOnlyCollection<DeliveryAddress> DeliveryAddresses { get; init; }
    public string PaymentMethod { get; init; }
    public PaymentCardValueObject PaymentCard { get; init; }
    public string? CardNumber { get; init; }
    public string? CardCvv { get; init; }
    public string? CardExpiration { get; init; }

    public SignUpCommand(
        string username,
        string password,
        string subscription,
        int phone,
        string city,
        string? fullName = null,
        string? address = null,
        IReadOnlyCollection<DeliveryAddress>? deliveryAddresses = null,
        string? paymentMethod = null,
        string? cardNumber = null,
        string? cardCvv = null,
        string? cardExpiration = null)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required.", nameof(username));
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password is required.", nameof(password));
        if (string.IsNullOrWhiteSpace(subscription)) throw new ArgumentException("Subscription is required.", nameof(subscription));
        if (phone <= 0) throw new ArgumentException("Phone must be greater than zero.", nameof(phone));
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City is required.", nameof(city));
        if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Address is required.", nameof(address));
        if (!string.IsNullOrWhiteSpace(paymentMethod) && paymentMethod.Length > 80)
            throw new ArgumentException("Payment method must be at most 80 characters.", nameof(paymentMethod));

        FullName = string.IsNullOrWhiteSpace(fullName) ? CreateDisplayName(username) : fullName.Trim();
        Username = username;
        Password = password;
        Subscription = UserSubscriptionPlan.NormalizeCode(subscription);
        Phone = phone;
        City = city.Trim();
        Address = address.Trim();
        CardNumber = cardNumber;
        CardCvv = cardCvv;
        CardExpiration = cardExpiration;
        DeliveryAddresses = DeliveryAddress.Normalize(deliveryAddresses, Address);
        PaymentCard = PaymentCardValueObject.FromRaw(cardNumber, cardCvv, cardExpiration);
        PaymentMethod = string.IsNullOrWhiteSpace(paymentMethod)
            ? PaymentCard.DisplayName
            : paymentMethod.Trim();
    }

    private static string CreateDisplayName(string username)
    {
        var emailName = username.Split('@', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? username;
        return emailName.Replace(".", " ").Replace("_", " ").Replace("-", " ").Trim();
    }
}
