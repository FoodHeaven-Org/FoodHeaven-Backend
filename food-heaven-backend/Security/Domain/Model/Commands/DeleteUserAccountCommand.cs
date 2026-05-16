namespace food_heaven_backend.Security.Domain.Model.Commands;

public record DeleteUserAccountCommand
{
    public int UserId { get; init; }

    public DeleteUserAccountCommand(int userId)
    {
        if (userId <= 0) throw new ArgumentException("User id must be greater than zero.", nameof(userId));

        UserId = userId;
    }
}
