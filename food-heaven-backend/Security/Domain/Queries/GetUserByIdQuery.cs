namespace food_heaven_backend.Security.Domain.Queries;

public record GetUserByIdQuery
{
    public GetUserByIdQuery(int userId)
    {
        UserId = userId;
    }

    public int UserId { get; init; }
}