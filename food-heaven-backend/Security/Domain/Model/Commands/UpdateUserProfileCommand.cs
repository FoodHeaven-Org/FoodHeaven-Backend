namespace food_heaven_backend.Security.Domain.Model.Commands;

public record UpdateUserProfileCommand
{
    public string FullName { get; init; }
    public string Username { get; init; }
    public int Phone { get; init; }
    public string City { get; init; }
    public string Address { get; init; }
    public string PaymentMethod { get; init; }

    public UpdateUserProfileCommand(
        string fullName,
        string username,
        int phone,
        string city,
        string? address = null,
        string? paymentMethod = null)
    {
        if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Full name is required.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required.", nameof(username));
        if (phone <= 0) throw new ArgumentException("Phone must be greater than zero.", nameof(phone));
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City is required.", nameof(city));
        if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Address is required.", nameof(address));
        if (string.IsNullOrWhiteSpace(paymentMethod)) throw new ArgumentException("Payment method is required.", nameof(paymentMethod));
        if (paymentMethod.Length > 80)
            throw new ArgumentException("Payment method must be at most 80 characters.", nameof(paymentMethod));

        FullName = fullName.Trim();
        Username = username.Trim();
        Phone = phone;
        City = city.Trim();
        Address = address.Trim();
        PaymentMethod = paymentMethod.Trim();
    }
}
