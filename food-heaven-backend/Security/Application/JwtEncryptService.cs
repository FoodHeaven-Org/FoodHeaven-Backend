using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using food_heaven_backend.Security.Domain.Entities;
using food_heaven_backend.Security.Domain.Service;
using Microsoft.IdentityModel.Tokens;

namespace food_heaven_backend.Security.Application
{
    public class JwtEncryptService : IJwtEncryptService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;

        public JwtEncryptService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Auth:key"]!));
        }

        public string Encrypt(User user)
        {
            // Agregar más claims según los campos de la entidad User
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),  // User ID
                new Claim(ClaimTypes.Name, user.Username),      // Username
                new Claim(ClaimTypes.Role, user.Subscription),  // Subscription
                new Claim("phone", user.Phone.ToString()),      // Phone
                new Claim("city", user.City)                    // City
            };

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(4),  // Expira en 4 horas
                SigningCredentials = credentials
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }

        public User Decrypt(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler();

            var principal = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero  // Para no permitir desajustes de tiempo
            }, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            // Obtener los valores de los claims
            var id = principal.FindFirst(ClaimTypes.Sid)?.Value;
            var username = principal.FindFirst(ClaimTypes.Name)?.Value;
            var subscription = principal.FindFirst(ClaimTypes.Role)?.Value;


            if (int.TryParse(id, out var userId) && username != null && subscription != null)
            {
                // Crear el usuario con los valores obtenidos
                return new User
                {
                    Id = userId,
                    Username = username,
                    Subscription = subscription,

                };
            }

            return null;
        }
    }
}
