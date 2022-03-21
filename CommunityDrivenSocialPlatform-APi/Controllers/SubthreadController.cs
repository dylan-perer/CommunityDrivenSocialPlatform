using CommunityDrivenSocialPlatform_APi.Data;
using CommunityDrivenSocialPlatform_APi.Model;
using CommunityDrivenSocialPlatform_APi.Service;
using CommunityDrivenSocialPlatform_APi.Validaton.Subthread;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CommunityDrivenSocialPlatform_APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubthreadController : ControllerBase
    {
        private readonly CDSPdB dbContext;
        public SubthreadController(CDSPdB dbContext)
        {
            this.dbContext = dbContext;
        }

        //handles returning all subthreads
        [HttpGet]
        public async Task<IActionResult> GetSubthread()
        {
            List<SubThread> subThreads = await dbContext.SubThread.ToListAsync();
            if (subThreads != null)
            {
                List<SubThreadDetailRequest> subThreadDetails = new List<SubThreadDetailRequest>();
                foreach (SubThread subThread in subThreads)
                {
                    subThreadDetails.Add(new SubThreadDetailRequest
                    {
                        Name = subThread.Name,
                        Description = subThread.Description,
                        WelcomeMessage = subThread.WelcomeMessage
                    });
                }
                return Ok(subThreadDetails);
            }
            else
                return NotFound("Sorry, No subthreads were found.");
        }

        //handles returning a single subthread
        [HttpGet("{name}")]
        public async Task<IActionResult> GetSubthreadByName([FromRoute] string name)
        {
            SubThread subThread = await dbContext.SubThread.FirstOrDefaultAsync(r => r.Name == name);
            if (subThread != null)
            {
                SubThreadDetailRequest subThreadDetail = new SubThreadDetailRequest
                {
                    Name = subThread.Name,
                    Description = subThread.Description,
                    WelcomeMessage = subThread.WelcomeMessage,
                };
                return Ok(subThreadDetail);
            }
            else
                return NotFound($"Sorry, no subthread with the name '{name}' exists. Please try again.");
        }

        //handles creating a subthread
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSubthread([FromBody] CreateSubThreadRequest createSubThreadRequest)
        {
            string loggedUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            User user = await dbContext.User.FirstOrDefaultAsync(r => r.Username == loggedUser);
            if (loggedUser != null && user != null)
            {
                try
                {
                    SubThread subThread = new SubThread
                    {//creating thread
                        Name = createSubThreadRequest.Name,
                        Description = createSubThreadRequest.Description,
                        WelcomeMessage = createSubThreadRequest.WelcomeMessage,
                        Creator = user.Id,
                        CreatedAt = DateTime.Now
                    };
                    dbContext.SubThread.Add(subThread);
                    await dbContext.SaveChangesAsync();

                    SubThreadUser subThreadUser = new SubThreadUser
                    {//giving creator of the subthread moderator role
                        SubThreadId = subThread.Id,
                        UserId = user.Id,
                        SubThreadRoleId = (int)SubthreadRoleEnum.MODERATOR,
                    };
                    dbContext.SubThreadUser.Add(subThreadUser);
                    await dbContext.SaveChangesAsync();

                    return Ok(createSubThreadRequest);
                }
                catch (Exception ex)
                {
                    return BadRequest("Sorry, that is malformed request. Please try again.");
                }
            }
            return BadRequest("Sorry, that is invalid request. Please try again.");
        }

        //handles updating subthread
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateByName([FromBody] UpdateSubThreadRequest updateSubThreadRequest)
        {
            string loggedUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (updateSubThreadRequest != null && loggedUser != null)
            {
                try
                {
                    SubThread subThread = await dbContext.SubThread.FirstOrDefaultAsync(r => r.Name == updateSubThreadRequest.Name);
                    if (subThread == null)
                        return BadRequest($"Sorry, you don't have the authorization to edit '{updateSubThreadRequest.Name}' subthread");

                    User user = await dbContext.User.FirstOrDefaultAsync(r => r.Username == loggedUser);
                    if (user != null)
                    {
                        SubThreadUser subThreadUser = await dbContext.SubThreadUser.FirstOrDefaultAsync(
                            r => r.SubThreadId == subThread.Id
                            && r.UserId == user.Id
                            && r.SubThreadRoleId == (int)SubthreadRoleEnum.MODERATOR);
                        if (subThreadUser != null)
                        {
                            subThread.Description = updateSubThreadRequest.Description;
                            subThread.WelcomeMessage = updateSubThreadRequest.WelcomeMessage;

                            dbContext.SubThread.Update(subThread);
                            await dbContext.SaveChangesAsync();
                            return Ok(updateSubThreadRequest);
                        }
                        return BadRequest($"Sorry, you don't have the authorization to edit '{updateSubThreadRequest.Name}' subthread");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest("Sorry, That is a invalid request. Please try again.");
                }
            }
            return BadRequest("Sorry, request body cannot be null.");
        }

        //delete subthread
        [Authorize]
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteByName([FromRoute] string name)
        {
            string loggedUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            User user = await dbContext.User.FirstOrDefaultAsync(r => r.Username == loggedUser);
            if (loggedUser != null && user != null)
            {
                SubThread subThread = await dbContext.SubThread.FirstOrDefaultAsync(r => r.Name == name);
                if (subThread == null)
                    return BadRequest($"Sorry, no subthread with the name '{name}' exists");

                if (subThread != null && user.Id == subThread.Creator)
                {
                    SubThreadUser subThreadUser = await dbContext.SubThreadUser.FirstOrDefaultAsync(r=> r.SubThreadId==subThread.Id && r.UserId == user.Id);
                    dbContext.SubThreadUser.Remove(subThreadUser);
                    await dbContext.SaveChangesAsync();

                    dbContext.SubThread.Remove(subThread);
                    await dbContext.SaveChangesAsync();
                    return Ok($"subthread '{name}' was successfully deleted.");
                }
                return BadRequest($"Sorry, only the creator of the subthread can delete.");
            }
            return BadRequest("Sorry, that is invalid request. Please try again.");
        }


        //join subthread

        //leave subthread

        //get all users of the subthread
        public class CreateSubThreadRequest
        {
            [EnsureUniqueSubthreadName]
            [StringLength(150, ErrorMessage = "Sorry, subthread name must be less than 150 characters. Please try again.")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Sorry, description is required. Please add a description and try again.")]
            public string Description { get; set; }

            [Required(ErrorMessage = "Sorry, welcome message is required. Please add a welcome message and try again.")]
            public string WelcomeMessage { get; set; }
        }

        public class SubThreadDetailRequest
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string WelcomeMessage { get; set; }
        }

        public class UpdateSubThreadRequest
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string WelcomeMessage { get; set; }
        }
    }

}

