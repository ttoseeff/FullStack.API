using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FullStack.API.Models
{
    public class JwtService
    {
        public string SecretKey { get; set; }
        public int TimeDuration { get; set; }

        private readonly IConfiguration config;

        public JwtService(IConfiguration _config)
        {
            config = _config;
            SecretKey = config.GetSection("jwtConfig").GetSection("Key").Value;
            TimeDuration = Int32.Parse(config.GetSection("jwtConfig").GetSection("Duration").Value);
        }

        public string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

            var signature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var payload = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
                new Claim("mobile", user.Mobile),
                new Claim("gender", user.Gender),
                new Claim("email", user.Email),
            };

            var jwtToken = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: payload,
                expires: DateTime.Now.AddMinutes(TimeDuration),
                signingCredentials: signature
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
