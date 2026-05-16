using food_heaven_backend.Shared.Domain.Model.Entities;

namespace food_heaven_backend.Security.Domain.Model.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;
    public string PasswordHashed { get; set; } = string.Empty;

    public string Subscription { get; set; } = string.Empty;
    
    public int Phone { get; set; }
    
    public string City { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string PaymentMethod { get; set; } = string.Empty;
}
