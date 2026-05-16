namespace food_heaven_backend.Security.Domain.Model.Commands;

public record LoginCommand
{
    public string Username { get; init; }
    public string Password { get; init; }

    public LoginCommand(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required.", nameof(username));
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password is required.", nameof(password));

        Username = username;
        Password = password;
    }
}
