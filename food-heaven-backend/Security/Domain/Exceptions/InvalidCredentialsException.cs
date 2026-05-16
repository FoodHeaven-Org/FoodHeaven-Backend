namespace food_heaven_backend.Security.Domain.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() : base("Invalid username or password") { }
}
