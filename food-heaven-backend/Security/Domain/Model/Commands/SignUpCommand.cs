namespace food_heaven_backend.Security.Domain.Model.Commands;

using food_heaven_backend.Security.Domain.Model.ValueObjects;

public record SignUpCommand
{
    public string FullName { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
    public string Subscription { get; init; }
    public int Phone { get; init; }
    public string City { get; init; }

    public SignUpCommand(string username, string password, string subscription, int phone, string city, string? fullName = null)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required.", nameof(username));
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password is required.", nameof(password));
        if (string.IsNullOrWhiteSpace(subscription)) throw new ArgumentException("Subscription is required.", nameof(subscription));
        if (phone <= 0) throw new ArgumentException("Phone must be greater than zero.", nameof(phone));
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City is required.", nameof(city));

        FullName = string.IsNullOrWhiteSpace(fullName) ? CreateDisplayName(username) : fullName.Trim();
        Username = username;
        Password = password;
        Subscription = UserSubscriptionPlan.NormalizeCode(subscription);
        Phone = phone;
        City = city;
    }

    private static string CreateDisplayName(string username)
    {
        var emailName = username.Split('@', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? username;
        return emailName.Replace(".", " ").Replace("_", " ").Replace("-", " ").Trim();
    }
}
