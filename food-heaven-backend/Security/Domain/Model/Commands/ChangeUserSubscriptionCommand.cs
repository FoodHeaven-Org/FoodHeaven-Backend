namespace food_heaven_backend.Security.Domain.Model.Commands;

using food_heaven_backend.Security.Domain.Model.ValueObjects;

public record ChangeUserSubscriptionCommand
{
    public string Subscription { get; init; }

    public ChangeUserSubscriptionCommand(string subscription)
    {
        if (string.IsNullOrWhiteSpace(subscription)) throw new ArgumentException("Subscription is required.", nameof(subscription));

        Subscription = UserSubscriptionPlan.NormalizeCode(subscription);
    }
}
