using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Backend.Entities;
using Backend.Services.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Identity;

namespace Backend.Services
{
    internal sealed class TokenProviderService(IConfiguration configuration, UserManager<User> userManager) : ITokenProviderService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly UserManager<User> _userManager = userManager;


        public string GenerateToken(User user)
        {
            string secretKey = _configuration["Jwt:Secret"]!;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim("email_confirmed", user.EmailConfirmed.ToString().ToLower())
    };

            // ✅ Add roles to claims
            var roles = _userManager.GetRolesAsync(user).Result;
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); // this is what makes [Authorize(Roles="Admin")] work
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
                SigningCredentials = credentials,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var handler = new JsonWebTokenHandler();
            string token = handler.CreateToken(tokenDescriptor);
            Console.WriteLine("Generated JWT: " + token);

            return token;
        }
    }
}
