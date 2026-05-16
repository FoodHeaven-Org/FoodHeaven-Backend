namespace food_heaven_backend.Security.Domain.Services;

public interface IHashService
{
    string HashPassword(string password);

    bool VerifyPassword(string password, string passwordHashed);
}