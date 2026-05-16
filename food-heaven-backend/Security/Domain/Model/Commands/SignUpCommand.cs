namespace food_heaven_backend.Security.Domain.Model.Commands;

public record SignUpCommand
{
    public string Username { get; init; }
    public string Password { get; init; }
    public string Subscription { get; init; }
    public int Phone { get; init; }
    public string City { get; init; }

    public SignUpCommand(string username, string password, string subscription, int phone, string city)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required.", nameof(username));
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password is required.", nameof(password));
        if (string.IsNullOrWhiteSpace(subscription)) throw new ArgumentException("Subscription is required.", nameof(subscription));
        if (phone <= 0) throw new ArgumentException("Phone must be greater than zero.", nameof(phone));
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City is required.", nameof(city));

        Username = username;
        Password = password;
        Subscription = subscription;
        Phone = phone;
        City = city;
    }
}
