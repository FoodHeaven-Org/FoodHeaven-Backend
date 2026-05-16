namespace food_heaven_backend.Security.Domain.Model.Commands;

public record ChangeUserPasswordCommand
{
    public string CurrentPassword { get; init; }
    public string NewPassword { get; init; }

    public ChangeUserPasswordCommand(string currentPassword, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(currentPassword)) throw new ArgumentException("Current password is required.", nameof(currentPassword));
        if (string.IsNullOrWhiteSpace(newPassword)) throw new ArgumentException("New password is required.", nameof(newPassword));
        if (newPassword.Length < 6) throw new ArgumentException("New password must contain at least 6 characters.", nameof(newPassword));

        CurrentPassword = currentPassword;
        NewPassword = newPassword;
    }
}
