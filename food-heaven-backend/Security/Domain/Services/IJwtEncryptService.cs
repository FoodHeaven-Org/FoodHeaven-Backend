using food_heaven_backend.Security.Domain.Model.Entities;

namespace food_heaven_backend.Security.Domain.Services;

public interface IJwtEncryptService
{
    string Encrypt(User user);
    User Decrypt(string encrypted);
}