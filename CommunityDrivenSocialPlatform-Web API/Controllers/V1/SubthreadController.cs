using CDSP_API.Contracts;
using CDSP_API.Contracts.V1.Requests;
using CDSP_API.Contracts.V1.Responses;
using CDSP_API.Data;
using CDSP_API.misc;
using CDSP_API.Model;
using CDSP_API.Models;
using CDSP_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CDSP_API.Controllers.V1
{
    [Route(ApiRoutes.BaseAndVersionV1 + ApiRoutes.Controller.SubThreadController)]
    [ApiController]
    public class SubthreadController : ControllerBase
    {
        private readonly ISubThreadsService _subthreadsService;
        private readonly IUsersService _usersService;
        private readonly ILogger<UserController> _logger;

        public SubthreadController(ISubThreadsService subthreadsService, IUsersService usersService, ILogger<UserController> logger)
        {
            _subthreadsService = subthreadsService;
            _usersService = usersService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            (EnityCoreResult ecr, List<SubThread> subThreads) = await _subthreadsService.GetAllAsync();
            if (!ecr.IsSuccess)
            {
                _logger.LogError(ecr.ToString("GetAllSubthreads"));
                return NotFound(ApiConstant.GenericError);
            }

            return Ok(new SubThreadDetailsResponse().MapToReponse(subThreads));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubThreadRequest createSubThreadRequest)
        {
            (EnityCoreResult ecr, User loggedUser) = await _usersService.GetLoggedUser(User);

            SubThread subThread = createSubThreadRequest.MapToModel();

            EnityCoreResult createEcr = await _subthreadsService.CreateAsync(subThread, loggedUser);

            if (!createEcr.IsSuccess)
            {
                _logger.LogError(ecr.ToString(createSubThreadRequest));
                return BadRequest(ApiConstant.GenericError);
            }
            return Ok(new SubThreadDetailsResponse().MapToReponse(subThread));
        }

        [HttpGet(ApiRoutes.Controller.RouteVariable.SubThreadName)]
        public async Task<IActionResult> GetByName([FromRoute] string subThreadName)
        {
            (EnityCoreResult ecr, SubThread subThread) = await _subthreadsService.GetByNameAsync(subThreadName);
            if (!ecr.IsSuccess)
            {
                _logger.LogError(ecr.ToString(subThreadName));
                return NotFound(ApiConstant.SubThread.NonExistentSubThread);
            }
            return Ok(new SubThreadDetailsResponse().MapToReponse(subThread));
        }

        [Authorize]
        [HttpPut(ApiRoutes.Controller.RouteVariable.SubThreadName)]
        public async Task<IActionResult> Update([FromRoute] string subThreadName, [FromBody] UpdateSubThreadRequest updateUserDetailsRequest)
        {
            (EnityCoreResult userEcr, User loggedUser) = await _usersService.GetLoggedUser(User);

            (EnityCoreResult ecr, SubThread subThread) = await _subthreadsService.GetByNameAsync(subThreadName);
            if (!ecr.IsSuccess)
            {
                _logger.LogError(ecr.ToString(updateUserDetailsRequest));
                return NotFound(ApiConstant.User.NonExistentUser);
            }
            subThread = updateUserDetailsRequest.MapToModel(subThread);

            EnityCoreResult updateEcr = await _subthreadsService.UpdateAsync(subThread,loggedUser);
            if (!updateEcr.IsSuccess)
            {
                _logger.LogError(ecr.ToString(updateUserDetailsRequest));
                return NotFound(ApiConstant.SubThread.FailedToUpdateSubThread);
            }

            return Ok(new SubThreadDetailsResponse().MapToReponse(subThread));
        }

        [Authorize]//<---
        [HttpDelete(ApiRoutes.Controller.RouteVariable.SubThreadName)]
        public async Task<IActionResult> Delete([FromRoute] string subThreadName)
        {
            (EnityCoreResult userEcr, User loggedUser) = await _usersService.GetLoggedUser(User);

            (EnityCoreResult ecr, SubThread subThread) = await _subthreadsService.GetByNameAsync(subThreadName);
            if (!ecr.IsSuccess)
            {
                _logger.Equals(ecr.ToString(subThreadName));
                return NotFound(ApiConstant.SubThread.NonExistentSubThread);
            }
            EnityCoreResult deleteEcr = await _subthreadsService.DeleteAsync(subThread, loggedUser);
            if (!deleteEcr.IsSuccess) return NotFound(ApiConstant.SubThread.FailedToUpdateSubThread);

            return Ok(ApiConstant.SubThread.SuccefullyDeletedSubThread);
        }

        [Authorize]
        [HttpGet(ApiRoutes.Controller.RouteVariable.SubThreadJoin)]
        public async Task<IActionResult> Join([FromRoute] string subThreadName)
        {
            (EnityCoreResult ecr, User loggedUser) = await _usersService.GetLoggedUser(User);

            (SubThread subThread, IActionResult action) = await IsValidSubThread(subThreadName);
            if (subThread is null) return action;

            (EnityCoreResult isMemberEcr, SubThreadUser subThreadUser) = await _subthreadsService.IsUserMember(subThread, loggedUser);
            if (isMemberEcr.IsSuccess && subThreadUser != null)
            {
                return NotFound(ApiConstant.SubThread.AlreadyAnMember);
            }

            EnityCoreResult joinEcr = null;
            if (subThread.CreatorId == loggedUser.Id)
            {
                joinEcr = await _subthreadsService.Join(subThread, loggedUser, SubThreadRoleEnum.USER);
            }
            else
            {
                joinEcr = await _subthreadsService.Join(subThread, loggedUser, SubThreadRoleEnum.USER);
            }
            if (!joinEcr.IsSuccess)
            {
                _logger.LogError(joinEcr.ToString(subThreadName));
                return BadRequest(ApiConstant.GenericError);
            }
            return Ok(ApiConstant.SubThread.JoinedSuccess);
        }

        [Authorize]
        [HttpGet(ApiRoutes.Controller.RouteVariable.SubThreadLeave)]
        public async Task<IActionResult> Leave([FromRoute] string subThreadName)
        {
            (EnityCoreResult ecr, User loggedUser) = await _usersService.GetLoggedUser(User);

            (SubThread subThread, IActionResult action) = await IsValidSubThread(subThreadName);
            if (subThread is null) return action;

            (EnityCoreResult isMemberEcr, SubThreadUser subThreadUser) = await _subthreadsService.IsUserMember(subThread, loggedUser);
            if (isMemberEcr.IsSuccess && subThreadUser == null)
            {
                return NotFound(ApiConstant.SubThread.NotAnMember);
            }

            EnityCoreResult leaveEcr = await _subthreadsService.Leave(subThreadUser, loggedUser);
            if (!leaveEcr.IsSuccess)
            {
                _logger.LogError(leaveEcr.ToString(subThreadName));
                return BadRequest(ApiConstant.GenericError);
            }

            return Ok(ApiConstant.SubThread.LeftSuccess);
        }

        [HttpGet(ApiRoutes.Controller.RouteVariable.SubThreadUsers)]
        public async Task<IActionResult> GetUsers([FromRoute] string subThreadName)
        {
            (var ecr, var users) = await _subthreadsService.SubThreadUsersAsync(subThreadName);
            if (!ecr.IsSuccess)
            {
                _logger.LogError(ecr.ToString(subThreadName));
                return BadRequest(ApiConstant.GenericError);
            }

            List<UserDetailsResponse> userDetailsResponses = new List<UserDetailsResponse>();
            foreach (var user in users)
            {
                userDetailsResponses.Add(new UserDetailsResponse().MapToReponse(user));
            }
            return Ok(userDetailsResponses);
        }

        private async Task<(SubThread, IActionResult)> IsValidSubThread(string subThreadName)
        {
            (EnityCoreResult ecr, SubThread subThread) = await _subthreadsService.GetByNameAsync(subThreadName);

            if (subThread is null)
                return (null, NotFound(ApiConstant.SubThread.NonExistentSubThread));
            return (subThread, null);
        }


    }
}
