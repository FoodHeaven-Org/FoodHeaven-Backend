using food_heaven_backend.Security.Domain.Entities;

namespace food_heaven_backend.Security.Domain.Service;

public interface IJwtEncryptService
{
    string Encrypt(User user);
    User Decrypt(string encrypted);
}