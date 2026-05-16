using System.Runtime.InteropServices.JavaScript;
using food_heaven_backend.Shared.Domain.Model.Entities;

namespace food_heaven_backend.Security.Domain.Entities;

public class User : BaseEntity
{
    public String Username { get; set; }
    public String PasswordHashed { get; set; }

    public String Subscription { get; set; }
    
    public int Phone { get; set; }
    
    public String City { get; set; }
    

    
    
}