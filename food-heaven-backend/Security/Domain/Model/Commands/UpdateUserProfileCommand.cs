namespace food_heaven_backend.Security.Domain.Model.Commands;

public record UpdateUserProfileCommand
{
    public string FullName { get; init; }
    public string Username { get; init; }
    public int Phone { get; init; }
    public string City { get; init; }

    public UpdateUserProfileCommand(string fullName, string username, int phone, string city)
    {
        if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Full name is required.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required.", nameof(username));
        if (phone <= 0) throw new ArgumentException("Phone must be greater than zero.", nameof(phone));
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City is required.", nameof(city));

        FullName = fullName.Trim();
        Username = username.Trim();
        Phone = phone;
        City = city.Trim();
    }
}
