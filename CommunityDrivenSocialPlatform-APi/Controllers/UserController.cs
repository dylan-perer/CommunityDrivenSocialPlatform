using CommunityDrivenSocialPlatform_APi.Data;
using CommunityDrivenSocialPlatform_APi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CommunityDrivenSocialPlatform_APi.Model.Request;
using CommunityDrivenSocialPlatform_APi.Model.Response;

namespace CommunityDrivenSocialPlatform_APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly CDSPdB DbContext;
        public UserController(CDSPdB dbContext)
        {
            DbContext = dbContext;
        }

        //:: handles returning all registered users :://
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<User> users = await DbContext.User.ToListAsync();
            if (users != null)
            {
                List<UserDetailResponse> usersDetails = new List<UserDetailResponse>();
                foreach (User user in users)
                {
                    usersDetails.Add(new UserDetailResponse
                    {
                        Username = user.Username,
                        Description = user.Description,
                        ProfilePictureUrl = user.ProfilePictureUrl
                    });
                }
                return Ok(usersDetails);
            }
            else
                return NotFound("Sorry, No users were found. Please try again.");
        }

        //:: handles returning a user by username :://
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUsername([FromRoute] string username)
        {
            User user = await DbContext.User.FirstOrDefaultAsync(r => r.Username == username);
            if (user != null)
            {
                UserDetailResponse userDetail = new UserDetailResponse
                {
                    Username = user.Username,
                    Description = user.Description,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                };
                return Ok(userDetail);
            }
            else
                return NotFound($"Sorry, no user with {username} exists. Please try again.");
        }

        //:: handles updating logged user :://
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDetailsRequest updateUserDetailsRequest)
        {
            string loggedUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;//get logged user's username
            User user = await DbContext.User.FirstOrDefaultAsync(r => r.Username == loggedUser);
            if (user != null)
            {
                user.ProfilePictureUrl = updateUserDetailsRequest?.ProfilePictureUrl;
                user.Description = updateUserDetailsRequest?.Description;

                if (updateUserDetailsRequest.EmailAddress != null)
                {
                    user.EmailAddress = updateUserDetailsRequest.EmailAddress;
                }
                else { updateUserDetailsRequest.EmailAddress = user.EmailAddress; }

                DbContext.User.Update(user);
                DbContext.SaveChanges();
                return Ok(updateUserDetailsRequest);
            }
            return BadRequest("Sorry, that is a malformed request.");
        }


/*        //:: handles getting posts user made :://
        [HttpGet("{username}/{posts}")]*/
    }
}

