using food_heaven_backend.Security.Domain.Model.Entities;
using food_heaven_backend.Security.Domain.Model.Queries;

namespace food_heaven_backend.Security.Domain.Services;

public interface IUserQueryService
{
    Task<User> Handle(GetUserByIdQuery query);
}