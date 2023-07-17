using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FullStack.API.Models
{
    public class LoginUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string responseMsg { get; set; }
        public string JwtToken { get; set; }

    }
}
