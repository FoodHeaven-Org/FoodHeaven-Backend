namespace food_heaven_backend.Security.Domain.Model.Commands;

public record SignUpCommand(String Username, String Password, String Subscription, int Phone, String City);