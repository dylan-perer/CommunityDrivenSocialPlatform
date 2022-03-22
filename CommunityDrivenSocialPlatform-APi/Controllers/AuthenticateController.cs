using CommunityDrivenSocialPlatform_APi.Data;
using CommunityDrivenSocialPlatform_APi.Model;
using CommunityDrivenSocialPlatform_APi.Validaton;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
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
        public async Task<IActionResult> Login([FromBody] loginRequest userLogin)//logs user in & generate token
        {
            //Authenticate user
            User user = await Authenticate(userLogin);

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
        public async Task<IActionResult> Signup([FromBody] signupRequest userSignup)
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
        private async Task<User> Authenticate(loginRequest userLogin)
        {
            if (userLogin != null)
            {
                User user = await DbContext.User.FirstOrDefaultAsync(user => user.Username == userLogin.Username && user.Password == userLogin.Password);
                if (user != null)
                {
                    Debug.WriteLine($"User found {user.Id}, {user.Username}");
                    return user;
                }
            }
            return null;

        }

        public class signupRequest
        {
            [SignupEnsureUniqueUsername]
            public string Username { get; set; }
            public string Password { get; set; }

            [SignupEnsureUniqueEmail]
            public string EmailAddress { get; set; }
            public string ProfilePictureUrl { get; set; } = null;

            public User createUser()
            {
                return new User
                {//map usersignup object to user model
                    Username = this.Username,
                    Password = this.Password,
                    EmailAddress = this.EmailAddress,
                    ProfilePictureUrl = this.ProfilePictureUrl,

                    CreatedAt = DateTime.Now,
                    RoleId = (int)RoleEnum.USER
                };
            }
        }

        public class loginRequest
        {
            [Required(ErrorMessage = "Username is required.")]
            [MinLength(3, ErrorMessage ="Sorry username must be atleast 3 characters long.")]
            [MaxLength(100, ErrorMessage ="Sorry username must not be more than 100 characters long.")]
            public string Username { get; set; }

            [MinLength(3, ErrorMessage = "Sorry username must be atleast 3 characters long.")]
            [MaxLength(255, ErrorMessage ="Sorry password must not be more than 255 characters long.")]
            public string Password { get; set; }
        }

    }
}
