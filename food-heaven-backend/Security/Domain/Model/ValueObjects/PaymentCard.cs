namespace food_heaven_backend.Security.Domain.Model.ValueObjects;

public sealed record PaymentCard(string Brand, string LastFour, string Expiration)
{
    public string DisplayName => string.IsNullOrWhiteSpace(LastFour)
        ? Brand
        : $"{Brand} ending in {LastFour}";

    public static PaymentCard FromRaw(string? cardNumber, string? cardCvv, string? cardExpiration)
    {
        var digits = OnlyDigits(cardNumber);
        var cvv = OnlyDigits(cardCvv);
        var expiration = NormalizeExpiration(cardExpiration);

        if (digits.Length is < 13 or > 19)
            throw new ArgumentException("Card number must contain between 13 and 19 digits.", nameof(cardNumber));

        if (!PassesLuhnCheck(digits))
            throw new ArgumentException("Card number is not valid.", nameof(cardNumber));

        if (cvv.Length is < 3 or > 4)
            throw new ArgumentException("Card security code must contain 3 or 4 digits.", nameof(cardCvv));

        return new PaymentCard(DetectBrand(digits), digits[^4..], expiration);
    }

    public static bool HasRawInput(string? cardNumber, string? cardCvv, string? cardExpiration)
    {
        return !string.IsNullOrWhiteSpace(cardNumber)
            || !string.IsNullOrWhiteSpace(cardCvv)
            || !string.IsNullOrWhiteSpace(cardExpiration);
    }

    private static string OnlyDigits(string? value)
    {
        return new string((value ?? string.Empty).Where(char.IsDigit).ToArray());
    }

    private static string NormalizeExpiration(string? value)
    {
        var digits = OnlyDigits(value);

        if (digits.Length == 3)
            digits = $"0{digits}";

        if (digits.Length != 4 && digits.Length != 6)
            throw new ArgumentException("Card expiration must use MM/YY or MM/YYYY format.", nameof(value));

        var month = int.Parse(digits[..2]);
        var year = digits.Length == 4
            ? 2000 + int.Parse(digits[2..])
            : int.Parse(digits[2..]);

        if (month is < 1 or > 12)
            throw new ArgumentException("Card expiration month is not valid.", nameof(value));

        var lastValidDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        if (lastValidDay < DateTime.UtcNow.Date)
            throw new ArgumentException("Card is expired.", nameof(value));

        return $"{month:00}/{year % 100:00}";
    }

    private static bool PassesLuhnCheck(string digits)
    {
        var sum = 0;
        var alternate = false;

        for (var index = digits.Length - 1; index >= 0; index--)
        {
            var value = digits[index] - '0';
            if (alternate)
            {
                value *= 2;
                if (value > 9) value -= 9;
            }

            sum += value;
            alternate = !alternate;
        }

        return sum % 10 == 0;
    }

    private static string DetectBrand(string digits)
    {
        if (digits.StartsWith('4')) return "Visa";
        if (digits.StartsWith("34") || digits.StartsWith("37")) return "American Express";
        if (digits.StartsWith("6011") || digits.StartsWith("65")) return "Discover";

        if (digits.Length >= 2)
        {
            var firstTwo = int.Parse(digits[..2]);
            if (firstTwo is >= 51 and <= 55) return "Mastercard";
        }

        if (digits.Length >= 4)
        {
            var firstFour = int.Parse(digits[..4]);
            if (firstFour is >= 2221 and <= 2720) return "Mastercard";
        }

        return "Card";
    }
}
