using food_heaven_backend.Security.Domain.Entities;
using food_heaven_backend.Security.Domain.Queries;

namespace food_heaven_backend.Security.Domain.Service;

public interface IUserQueryService
{
    Task<User> Handle(GetUserByIdQuery query);
}