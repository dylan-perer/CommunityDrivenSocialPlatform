using CommunityDrivenSocialPlatform_APi.Data;
using CommunityDrivenSocialPlatform_APi.Data.Request;
using CommunityDrivenSocialPlatform_APi.Data.Response;
using CommunityDrivenSocialPlatform_APi.Model;
using CommunityDrivenSocialPlatform_APi.Model.Request;
using CommunityDrivenSocialPlatform_APi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        //:: handles returning all subthreads :://
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

        //:: handles returning a subthread by name :://
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

        //:: handles creating a subthread :://
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSubthread([FromBody] CreateSubThreadRequest createSubThreadRequest)
        {
            User user = await GetLoggedUser();
            SubThread subThread = new SubThread
            {//creating thread
                Name = createSubThreadRequest.Name,
                Description = createSubThreadRequest.Description,
                WelcomeMessage = createSubThreadRequest.WelcomeMessage,
                Creator = user.Id,
                CreatedAt = DateTime.Now,
            };

            subThread.SubThreadUser.Add(new SubThreadUser
            {//giving creator of the subthread moderator role
                SubThreadId = subThread.Id,
                UserId = user.Id,
                SubThreadRoleId = (int)SubthreadRoleEnum.MODERATOR,
            });

            dbContext.SubThread.Add(subThread);
            await dbContext.SaveChangesAsync();

            return Ok(createSubThreadRequest);
        }

        //:: handles updating a subthread :://
        [Authorize]
        [HttpPut("{name}")]
        public async Task<IActionResult> UpdateByName([FromRoute] string name, [FromBody] UpdateSubThreadRequest updateSubThreadRequest)
        {
            User user = await GetLoggedUser();

            SubThread subThread = await GetSubThread(name);
            if (subThread == null)
                return BadRequest(Constants.SubthreadUpdatedSuccessfully);

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
            return BadRequest(Constants.NotAuthorizedToEditSubthread(name));
        }

        //:: handles deleting a subthread :://
        [Authorize]
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteByName([FromRoute] string name)
        {
            User user = await GetLoggedUser();
            SubThread subThread = await GetSubThread(name);
            if (subThread == null)
                return BadRequest(Constants.NonExistentSubThread(name));

            if (user.Id == subThread.Creator)
            {
                dbContext.SubThread.Remove(subThread);
                await dbContext.SaveChangesAsync();
                return Ok(Constants.SubthreadDeletedSuccessfully);
            }
            return BadRequest(Constants.NoAuthorization);
        }

        //:: handles joining a subthread :://
        [Authorize]
        [HttpGet("{name}/join")]
        public async Task<IActionResult> JoinSubthread([FromRoute] string name)
        {
            User user = await GetLoggedUser();

            SubThread subThread = await GetSubThread(name);
            if (subThread == null)
                return BadRequest(Constants.NonExistentSubThread(name));

            SubThreadUser subThreadUser = await dbContext.SubThreadUser.FirstOrDefaultAsync(r => r.SubThreadId == subThread.Id && r.UserId == user.Id);

            if (subThreadUser != null)
                return Ok(Constants.AlreadyMemberOfSubthread(name));

            dbContext.SubThreadUser.Add(new SubThreadUser
            {
                SubThreadId = subThread.Id,
                UserId = user.Id,
                SubThreadRoleId = subThread.Creator == user.Id ? (int)SubthreadRoleEnum.MODERATOR : (int)SubthreadRoleEnum.USER
            });

            await dbContext.SaveChangesAsync();
            return Ok(Constants.JoinedNewSubthread(name));
        }

        //:: handles leaving a subthread :://
        [Authorize]
        [HttpGet("{name}/leave")]
        public async Task<IActionResult> LeaveSubthread([FromRoute] string name)
        {
            User user = await GetLoggedUser();
            SubThread subThread = await GetSubThread(name);
            if (subThread == null)
                return BadRequest(Constants.NonExistentSubThread(name));

            SubThreadUser subThreadUser = await dbContext.SubThreadUser.FirstOrDefaultAsync(r => r.SubThreadId == subThread.Id && r.UserId == user.Id);
            
            if (subThreadUser != null)
            {
                dbContext.SubThreadUser.Remove(subThreadUser);
                await dbContext.SaveChangesAsync();
                return Ok(Constants.LeftSubthread(name));
            }

            return Ok(Constants.NotMemberOfSubthread(name));
        }

        //:: handles returning all joined users of a subthread :://
        [HttpGet("{name}/users")]
        public async Task<IActionResult> GetAllUsers([FromRoute] string name)
        {
            SubThread subThread = await dbContext.SubThread.FirstOrDefaultAsync(r => r.Name == name);
            if (subThread != null)
            {
                var innerJoin = dbContext.User.Join(dbContext.SubThreadUser, tbl_user => tbl_user.Id, tbl_sub_thread_user => tbl_sub_thread_user.UserId,
                    (tbl_user, tbl_sub_thread_user) => new { tbl_user, tbl_sub_thread_user })
                    .Select(c => new { c.tbl_user.Username, c.tbl_sub_thread_user.SubThreadRoleId, c.tbl_sub_thread_user.SubThreadId }).Where(r => r.SubThreadId == subThread.Id);

                List<SubThreadUserDetailResponse> subThreadUserRequests = new List<SubThreadUserDetailResponse>();
                if (subThreadUserRequests != null)
                {
                    foreach (var item in innerJoin)
                    {
                        subThreadUserRequests.Add(new SubThreadUserDetailResponse
                        {
                            Username = item.Username,
                            RoleId = item.SubThreadRoleId
                        });
                    }
                    if (subThreadUserRequests.Count > 0)
                    {
                        return Ok(subThreadUserRequests);
                    }
                    return Ok($"There are no members in '{name}' subthread.");
                }
            }
            return BadRequest($"Sorry, subthread with the name '{name}' does not exist.");
        }

        public async Task<User> GetLoggedUser()
        {
            string loggedUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return await dbContext.User.FirstOrDefaultAsync(r => r.Username == loggedUser);
        }

        public async Task<SubThread> GetSubThread(string name)
        {
            return await dbContext.SubThread.FirstOrDefaultAsync(r => r.Name == name);
        }
    }

}

