using BlogApi.Models.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace BlogApi.Auth
{
    public class JwtHelper
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtHelper(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("JWT Secret Key not found.");
            _issuer = configuration["Jwt:Issuer"] ?? "BlogApi";
            _audience = configuration["Jwt:Audience"] ?? "BlogApiUsers";
        }

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role) // 👈 Agregamos el rol
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
