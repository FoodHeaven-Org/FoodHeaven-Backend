namespace food_heaven_backend.Security.Domain.Model.Queries;

public record GetUserByIdQuery
{
    public int UserId { get; init; }

    public GetUserByIdQuery(int userId)
    {
        if (userId <= 0) throw new ArgumentException("UserId must be greater than zero.", nameof(userId));
        UserId = userId;
    }
}
