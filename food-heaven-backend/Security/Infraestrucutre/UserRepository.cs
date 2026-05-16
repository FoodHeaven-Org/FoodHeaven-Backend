using food_heaven_backend.Security.Domain.Entities;
using food_heaven_backend.Security.Domain.Repositories;
using food_heaven_backend.Shared.Infraestructure.Persistence.Configuration;
using food_heaven_backend.Shared.Infraestructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace food_heaven_backend.Security.Infraestrucutre;

public class UserRepository(FoodHeavenContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByUsernamelAsync(string username)
    {
        return await context.Set<User>().FirstOrDefaultAsync(u => u.Username == username);
    }
}