using food_heaven_backend.Security.Domain.Model.Entities;
using food_heaven_backend.Security.Domain.Repositories;
using food_heaven_backend.Shared.Infrastructure.Persistence.EfCore.Configuration;
using food_heaven_backend.Shared.Infrastructure.Persistence.EfCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace food_heaven_backend.Security.Infrastructure.Persistence.EfCore.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(FoodHeavenContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByUsernameExceptIdAsync(string username, int userId)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(u => u.Username == username && u.Id != userId);
    }
}
