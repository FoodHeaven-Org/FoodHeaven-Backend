namespace food_heaven_backend.Security.Domain.Model.Exceptions;

public class UsernameAlreadyTakenException : Exception
{
    public UsernameAlreadyTakenException() : base("Username already taken") { }
}
