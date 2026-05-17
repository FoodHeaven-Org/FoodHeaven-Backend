namespace food_heaven_backend.Security.Domain.Model.ValueObjects;

public sealed record DeliveryAddress
{
    public string Label { get; init; }
    public string AddressLine { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public bool IsDefault { get; init; }

    public DeliveryAddress(
        string? label,
        string addressLine,
        double? latitude = null,
        double? longitude = null,
        bool isDefault = false)
    {
        if (string.IsNullOrWhiteSpace(addressLine))
            throw new ArgumentException("Delivery address is required.", nameof(addressLine));

        if (latitude is < -90 or > 90)
            throw new ArgumentException("Latitude is not valid.", nameof(latitude));

        if (longitude is < -180 or > 180)
            throw new ArgumentException("Longitude is not valid.", nameof(longitude));

        Label = string.IsNullOrWhiteSpace(label) ? "Principal" : label.Trim();
        AddressLine = addressLine.Trim();
        Latitude = latitude;
        Longitude = longitude;
        IsDefault = isDefault;
    }

    public static IReadOnlyCollection<DeliveryAddress> Normalize(
        IEnumerable<DeliveryAddress>? deliveryAddresses,
        string fallbackAddress)
    {
        var addresses = deliveryAddresses?
            .Where(item => !string.IsNullOrWhiteSpace(item.AddressLine))
            .Select(item => new DeliveryAddress(
                item.Label,
                item.AddressLine,
                item.Latitude,
                item.Longitude,
                item.IsDefault))
            .ToList() ?? [];

        if (addresses.Count == 0)
        {
            addresses.Add(new DeliveryAddress("Principal", fallbackAddress, isDefault: true));
        }

        if (addresses.All(item => !item.IsDefault))
        {
            addresses[0] = addresses[0] with { IsDefault = true };
        }

        var defaultAlreadySet = false;
        for (var index = 0; index < addresses.Count; index++)
        {
            if (!addresses[index].IsDefault) continue;

            if (!defaultAlreadySet)
            {
                defaultAlreadySet = true;
                continue;
            }

            addresses[index] = addresses[index] with { IsDefault = false };
        }

        return addresses;
    }
}
