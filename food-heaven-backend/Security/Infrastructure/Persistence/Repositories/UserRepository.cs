using food_heaven_backend.Security.Domain.Model.Entities;
using food_heaven_backend.Security.Domain.Repositories;
using food_heaven_backend.Shared.Infrastructure.Persistence.Configuration;
using food_heaven_backend.Shared.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace food_heaven_backend.Security.Infrastructure.Persistence.Repositories;

public class UserRepository(FoodHeavenContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByUsernamelAsync(string username)
    {
        return await context.Set<User>().FirstOrDefaultAsync(u => u.Username == username);
    }
}