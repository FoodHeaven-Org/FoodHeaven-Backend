namespace food_heaven_backend.Security.Domain.Service;

public interface IHashService
{
    string HashPassword(string password);

    bool VerifyPassword(string password, string passwordHashed);
}