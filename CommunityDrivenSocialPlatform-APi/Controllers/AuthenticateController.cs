using CommunityDrivenSocialPlatform_APi.Data;
using CommunityDrivenSocialPlatform_APi.Model;
using CommunityDrivenSocialPlatform_APi.Model.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CommunityDrivenSocialPlatform_APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly CDSPdB DbContext;

        public AuthenticateController(IConfiguration configuration, CDSPdB dbContext)//get depedecients
        {
            Configuration = configuration;
            DbContext = dbContext;
        }

        //:: handles loggin in and generating a jwt token :://
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)//logs user in & generate token
        {
            //Authenticate user
            User user = await Authenticate(loginRequest);

            //generate token
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }
            return NotFound("Sorry, Username or Password was incorrect. Please try again.");
        }

        //:: handles signing up a user :://
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest userSignup)
        {
            if (userSignup != null)
            {
                await DbContext.User.AddAsync(userSignup.createUser());
                DbContext.SaveChanges();
                return Ok(userSignup);
            }
            return BadRequest("Sorry, Invalid request. Please try again");
        }

        //:: handles generating a jwt token :://
        private string GenerateToken(User user)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            SigningCredentials credintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Role role = DbContext.Role.FirstOrDefault(r => user.RoleId == r.Id);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, role.RoleName),
            };

            JwtSecurityToken token = new JwtSecurityToken(
                Configuration["Jwt:Issuer"],
                Configuration["Jwt:Audience"],
                claims, expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credintials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //:: handles calling datasource to authenticate a user :://
        private async Task<User> Authenticate(LoginRequest loginRequest)
        {
            if (loginRequest != null)
            {
                User user = await DbContext.User.FirstOrDefaultAsync(user => user.Username == loginRequest.Username && user.Password == loginRequest.Password);
                if (user != null)
                {
                    Debug.WriteLine($"User found {user.Id}, {user.Username}");
                    return user;
                }
            }
            return null;

        }

    }
}
