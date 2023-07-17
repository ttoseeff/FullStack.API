using FullStack.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStack.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private FullStackDbContext _context;
        private IConfiguration _config;
        public UserController(FullStackDbContext context, IConfiguration config)
        {
            this._context = context;
            this._config = config;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            User Exist = this._context.Users.FirstOrDefault(x => x.Email == user.Email);
            if (Exist != null)
            {
                return Ok(user);
            }
            user.MemberSince = DateTime.Now;
            await this._context.Users.AddAsync(user);
            await this._context.SaveChangesAsync();  
            return Ok(user);

        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginUser user)
        {
            var UserExist = await this._context.Users.FirstOrDefaultAsync(x=> x.Email == user.Email && x.Password == user.Password  );
            if (UserExist != null)
            {
                user.responseMsg = "Success";
                user.JwtToken = new JwtService(_config).GenerateToken(UserExist);
                return Ok(user);
            }
            else
            {
                user.responseMsg = "Failure";
                return Ok(user);
            }
        }
    }
}
