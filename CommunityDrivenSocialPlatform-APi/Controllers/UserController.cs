using CommunityDrivenSocialPlatform_APi.Data;
using CommunityDrivenSocialPlatform_APi.Model;
using CommunityDrivenSocialPlatform_APi.Service;
using CommunityDrivenSocialPlatform_APi.Validaton;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static CommunityDrivenSocialPlatform_APi.Controllers.AuthenticateController;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System;

namespace CommunityDrivenSocialPlatform_APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly CDSPdB dbContext;
        public UserController(CDSPdB dbContext)
        {
            this.dbContext = dbContext;
        }

        //handles returning all registered users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<User> users = await dbContext.User.ToListAsync();
            if (users != null)
            {
                List<UserDetail> usersDetails = new List<UserDetail>();
                foreach (User user in users)
                {
                    usersDetails.Add(new UserDetail
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


        //handles returning a single user
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUsername([FromRoute] string username)
        {
            User user = await dbContext.User.FirstOrDefaultAsync(r => r.Username == username);
            if (user != null)
            {
                UserDetail userDetail = new UserDetail
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

        [HttpPut]
        [Authorize(Roles = "USER, ADMIN")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDetailsRequest updateUserDetailsRequest)
        {
            string loggedUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;//get logged user's username
            if (loggedUser != null)
            {
                try
                {
                    User user = await dbContext.User.FirstOrDefaultAsync(r => r.Username == loggedUser);
                    if (user != null)
                    {
                        user.ProfilePictureUrl = updateUserDetailsRequest.ProfilePictureUrl;
                        user.Description = updateUserDetailsRequest.Description;

                        if (updateUserDetailsRequest.EmailAddress != null)
                        {
                            user.EmailAddress = updateUserDetailsRequest.EmailAddress;
                        }

                        user.Description = updateUserDetailsRequest.Description;
                        dbContext.User.Update(user);
                        dbContext.SaveChanges();
                        return Ok(updateUserDetailsRequest);
                    }
                }catch(Exception ex)
                {
                    return BadRequest("Sorry, that is a malformed request.");
                }
            }
            return BadRequest("Sorry, you must be logged in to be update details. Please login and again.");
        }

    }
    public class UserDetail
    {
        public string Username { get; set; }
        public string Description { get; set; }
        public string ProfilePictureUrl { get; set; }
    }

    public class UpdateUserDetailsRequest
    {
        public string Description { get; set; }
        public string ProfilePictureUrl { get; set; }
        [UserSignupEnsureUniqueEmail]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}

