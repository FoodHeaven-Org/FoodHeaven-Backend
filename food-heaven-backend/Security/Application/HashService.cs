using food_heaven_backend.Security.Domain.Service;
using Org.BouncyCastle.Crypto.Generators;

namespace food_heaven_backend.Security.Application;

public class HashService : IHashService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string passwordHashed)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHashed);
    }
}