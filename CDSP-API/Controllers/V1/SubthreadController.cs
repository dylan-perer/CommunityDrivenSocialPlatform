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
    [Route("api/v1/[controller]")]
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
            (EnityCoreResult ecr,List<SubThread> subThreads) = await _subthreadsService.GetAllAsync();
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
            if (!ecr.IsSuccess)
            {
                return BadRequest("You are not logged in.");
            }

            SubThread subThread = createSubThreadRequest.MapToModel();

            EnityCoreResult createEcr = await _subthreadsService.CreateAsync(subThread, loggedUser);

            if (!createEcr.IsSuccess)
            {
                _logger.LogError(ecr.ToString(createSubThreadRequest));
                return BadRequest(ApiConstant.GenericError);
            }
            return Ok(new SubThreadDetailsResponse().MapToReponse(subThread));
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName([FromRoute] string name)
        {
            (EnityCoreResult ecr, SubThread subThread) = await _subthreadsService.GetByNameAsync(name);
            if (!ecr.IsSuccess)
            {
                _logger.LogError(ecr.ToString(name));
                return NotFound(ApiConstant.SubThread.NonExistentSubThread);
            }
            return Ok(new SubThreadDetailsResponse().MapToReponse(subThread));
        }

        [Authorize]
        [HttpPut("{name}")]//<--auth
        public async Task<IActionResult> Update([FromRoute] string name, [FromBody] UpdateSubThreadRequest updateUserDetailsRequest)
        {
            (EnityCoreResult ecr, SubThread subThread) = await _subthreadsService.GetByNameAsync(name);
            if (!ecr.IsSuccess)
            {
                _logger.LogError(ecr.ToString(updateUserDetailsRequest));
                return NotFound(ApiConstant.User.NonExistentUser);
            }
            subThread = updateUserDetailsRequest.MapToModel(subThread);

            EnityCoreResult updateEcr = await _subthreadsService.UpdateAsync(subThread);
            if (!updateEcr.IsSuccess)
            {
                _logger.LogError(ecr.ToString(updateUserDetailsRequest));
                return NotFound(ApiConstant.SubThread.FailedToUpdateSubThread);
            }

            return Ok(new SubThreadDetailsResponse().MapToReponse(subThread));
        }

        [Authorize]//<---
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete([FromRoute] string name)
        {
            (EnityCoreResult ecr, SubThread subThread) = await _subthreadsService.GetByNameAsync(name);
            if (!ecr.IsSuccess)
            {
                _logger.Equals(ecr.ToString(name));
                return NotFound(ApiConstant.SubThread.NonExistentSubThread);
            }
            EnityCoreResult deleteEcr = await _subthreadsService.DeleteAsync(subThread);
            if (!deleteEcr.IsSuccess) return NotFound(ApiConstant.SubThread.FailedToUpdateSubThread);

            return Ok(ApiConstant.SubThread.SuccefullyDeletedSubThread);
        }

        [Authorize]
        [HttpGet("{name}/join")]
        public async Task<IActionResult> Join([FromRoute] string name)
        {
            (EnityCoreResult ecr,User loggedUser) = await _usersService.GetLoggedUser(User);

            (SubThread subThread, IActionResult action) = await IsValidSubThread(name);
            if (subThread is null) return action;

            EnityCoreResult isMemberEcr = await _subthreadsService.IsUserMember(subThread, loggedUser);
            if (!isMemberEcr.IsSuccess)
            {
                _logger.LogError(isMemberEcr.ToString(name));
                return NotFound(ApiConstant.SubThread.AlreadyAnMember);
            }
            EnityCoreResult joinEcr = await _subthreadsService.Join(subThread, loggedUser, SubThreadRoleEnum.USER);
            if (!joinEcr.IsSuccess)
            {
                _logger.LogError(joinEcr.ToString(name));
                return BadRequest(ApiConstant.GenericError);
            }
            return Ok(ApiConstant.SubThread.JoinedSuccess);
        }

        [Authorize]
        [HttpGet("{name}/leave")]
        public async Task<IActionResult> Leave([FromRoute] string name)
        {
            (EnityCoreResult ecr,User loggedUser) = await _usersService.GetLoggedUser(User);

            (SubThread subThread, IActionResult action) = await IsValidSubThread(name);
            if (subThread is null) return action;

            EnityCoreResult isMemberEcr = await _subthreadsService.IsUserMember(subThread, loggedUser);
            if (!isMemberEcr.IsSuccess)
            {
                _logger.LogError(isMemberEcr.ToString(name));
                return NotFound(ApiConstant.SubThread.NotAnMember);
            }

            EnityCoreResult leaveEcr = await _subthreadsService.Leave(subThread, loggedUser);
            if (!leaveEcr.IsSuccess)
            {
                _logger.LogError(leaveEcr.ToString(name));
                return BadRequest(ApiConstant.GenericError);
            }

            return Ok(ApiConstant.SubThread.LeftSuccess);
        }

        private async Task<(SubThread, IActionResult)> IsValidSubThread(string name)
        {
            (EnityCoreResult ecr, SubThread subThread) = await _subthreadsService.GetByNameAsync(name);

            if (subThread is null)
                return (null, NotFound(ApiConstant.SubThread.NonExistentSubThread));
            return (subThread, null);
        }


    }
}
