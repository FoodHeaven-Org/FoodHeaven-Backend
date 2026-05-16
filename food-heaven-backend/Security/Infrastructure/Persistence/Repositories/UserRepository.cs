using food_heaven_backend.Security.Domain.Model.Entities;
using food_heaven_backend.Security.Domain.Repositories;
using food_heaven_backend.Shared.Infrastructure.Persistence.Configuration;
using food_heaven_backend.Shared.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace food_heaven_backend.Security.Infrastructure.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(FoodHeavenContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(u => u.Username == username);
    }
}
