using food_heaven_backend.Security.Domain.Entities;
using food_heaven_backend.Shared.Domain.Repositories;

namespace food_heaven_backend.Security.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByUsernamelAsync(string usernamel);
}