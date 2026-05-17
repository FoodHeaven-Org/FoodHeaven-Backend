using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using food_heaven_backend.Security.Domain.Model.Entities;
using food_heaven_backend.Security.Domain.Services;
using Microsoft.IdentityModel.Tokens;

namespace food_heaven_backend.Security.Infrastructure.Tokens
{
    public class JwtEncryptService : IJwtEncryptService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;

        public JwtEncryptService(IConfiguration configuration)
        {
            _configuration = configuration;
            var authKey = _configuration["Auth:key"] ?? throw new InvalidOperationException("Auth:key configuration is required.");
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey));
        }

        public string Encrypt(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim("fullName", user.FullName),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Subscription),
                new Claim("phone", user.Phone.ToString()),
                new Claim("city", user.City),
                new Claim("address", user.Address),
                new Claim("paymentCardBrand", user.PaymentCardBrand),
                new Claim("paymentCardLastFour", user.PaymentCardLastFour),
                new Claim("paymentCardExpiration", user.PaymentCardExpiration)
            };

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = credentials
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }

        public User? Decrypt(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler();

            ClaimsPrincipal principal;
            SecurityToken validatedToken;

            try
            {
                principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out validatedToken);
            }
            catch (SecurityTokenException)
            {
                return null;
            }

            if (validatedToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            var id = principal.FindFirst(ClaimTypes.Sid)?.Value;
            var fullName = principal.FindFirst("fullName")?.Value;
            var username = principal.FindFirst(ClaimTypes.Name)?.Value;
            var subscription = principal.FindFirst(ClaimTypes.Role)?.Value;

            if (int.TryParse(id, out var userId) && username != null && subscription != null)
            {
                return new User
                {
                    Id = userId,
                    FullName = fullName ?? string.Empty,
                    Username = username,
                    Subscription = subscription
                };
            }

            return null;
        }
    }
}
