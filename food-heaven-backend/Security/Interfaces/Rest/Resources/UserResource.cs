using food_heaven_backend.Security.Domain.Model.Entities;

namespace food_heaven_backend.Security.Interfaces.Rest.Resources;

public record UserResource(
    int Id,
    string FullName,
    string Username,
    string Subscription,
    int Phone,
    string City
)
{
    public static UserResource FromEntity(User user)
    {
        return new UserResource(
            user.Id,
            user.FullName,
            user.Username,
            user.Subscription,
            user.Phone,
            user.City
        );
    }
}
