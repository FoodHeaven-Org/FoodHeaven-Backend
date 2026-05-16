namespace food_heaven_backend.Security.Domain.Model.ValueObjects;

public sealed record UserSubscriptionPlan(
    string Code,
    string DisplayName,
    int MealsPerDay,
    decimal MonthlyPrice)
{
    public static readonly UserSubscriptionPlan Essential = new("Essential", "Plan Esencial", 1, 149m);
    public static readonly UserSubscriptionPlan Balance = new("Balance", "Plan Balance", 2, 249m);
    public static readonly UserSubscriptionPlan Full = new("Full", "Plan Full", 3, 349m);

    public static IReadOnlyCollection<UserSubscriptionPlan> AvailablePlans { get; } =
    [
        Essential,
        Balance,
        Full
    ];

    public static UserSubscriptionPlan FromCode(string code)
    {
        var normalizedCode = NormalizeCode(code);
        var plan = AvailablePlans.FirstOrDefault(item => item.Code == normalizedCode);

        return plan ?? throw new ArgumentException("Subscription plan is not supported.", nameof(code));
    }

    public static string NormalizeCode(string code)
    {
        return code.Trim().ToLowerInvariant() switch
        {
            "essential" or "standard" => Essential.Code,
            "balance" or "breakfastlunch" or "lunchdinner" => Balance.Code,
            "full" => Full.Code,
            _ => throw new ArgumentException("Subscription plan is not supported.", nameof(code))
        };
    }
}
