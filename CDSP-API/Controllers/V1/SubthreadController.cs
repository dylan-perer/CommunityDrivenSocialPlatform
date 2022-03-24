using CDSP_API.Contracts.V1.Requests;
using CDSP_API.Contracts.V1.Responses;
using CDSP_API.misc;
using CDSP_API.Model;
using CDSP_API.Models;
using CDSP_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CDSP_API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SubthreadController : ControllerBase
    {
        private readonly ISubThreadsService _subthreadsService;
        private readonly IUsersService _usersService;
        public SubthreadController(ISubThreadsService subthreadsService, IUsersService usersService)
        {
            _subthreadsService = subthreadsService;
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subThreads = await _subthreadsService.GetAllAsync();

            return Ok(new SubThreadDetailsResponse().MapToReponse(subThreads));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubThreadRequest createSubThreadRequest)
        {
            User loggedUser = await _usersService.GetLoggedUser(User);

            SubThread subThread = createSubThreadRequest.MapToModel();
            
            bool isSuccess = await _subthreadsService.CreateAsync(subThread, loggedUser);
            if (isSuccess is false) return BadRequest(ApiConstant.GenericError);

            return Ok(new SubThreadDetailsResponse().MapToReponse(subThread));
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName([FromRoute] string name)
        {
            var subThread = await _subthreadsService.GetByNameAsync(name);
            if (subThread == null) return NotFound(ApiConstant.SubThread.NonExistentSubThread); 

            return Ok(new SubThreadDetailsResponse().MapToReponse(subThread));
        }

        [Authorize]
        [HttpPut("{name}")]//<--
        public async Task<IActionResult> Update([FromRoute] string name, [FromBody] UpdateSubThreadRequest updateUserDetailsRequest)
        {
            var subThread = await _subthreadsService.GetByNameAsync(name);
            if (subThread == null) return NotFound(ApiConstant.User.NonExistentUser);

            subThread = updateUserDetailsRequest.MapToModel(subThread);

            bool isSuccess = await _subthreadsService.UpdateAsync(subThread);
            if (!isSuccess) return NotFound(ApiConstant.SubThread.FailedToUpdateSubThread);

            return Ok(new SubThreadDetailsResponse().MapToReponse(subThread));
        }

        [Authorize]//<---
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete([FromRoute] string name)
        {
            var subThread = await _subthreadsService.GetByNameAsync(name);
            if (subThread == null) return NotFound(ApiConstant.SubThread.NonExistentSubThread);

            bool isSuccess = await _subthreadsService.DeleteAsync(subThread);
            if (!isSuccess) return NotFound(ApiConstant.SubThread.FailedToUpdateSubThread);

            return Ok(ApiConstant.SubThread.SuccefullyDeletedSubThread);
        }

        [Authorize]
        [HttpGet("{name}/join")]
        public async Task<IActionResult> Join([FromRoute] string name)
        {
            User loggedUser = await _usersService.GetLoggedUser(User);

            (SubThread subThread, IActionResult action) = await IsValidSubThread(name);
            if (subThread is null) return action;

            bool isAlreadyUser = await _subthreadsService.IsUserMember(subThread, loggedUser);
            if (isAlreadyUser) return BadRequest(ApiConstant.SubThread.AlreadyAnMember);

            bool isSuccess = await _subthreadsService.Join(subThread, loggedUser, SubThreadRoleEnum.USER);
            if(!isSuccess) return BadRequest(ApiConstant.GenericError);
            
            return Ok(ApiConstant.SubThread.JoinedSuccess);
        }

        [Authorize]
        [HttpGet("{name}/leave")]
        public async Task<IActionResult> Leave([FromRoute] string name)
        {
            User loggedUser = await _usersService.GetLoggedUser(User);

            (SubThread subThread, IActionResult action) = await IsValidSubThread(name);
            if (subThread is null) return action;

            bool isAlreadyUser = await _subthreadsService.IsUserMember(subThread, loggedUser);
            if (!isAlreadyUser) return BadRequest(ApiConstant.SubThread.NotAnMember);

            bool isSuccess = await _subthreadsService.Leave(subThread, loggedUser);
            if (!isSuccess) return BadRequest(ApiConstant.GenericError);

            return Ok(ApiConstant.SubThread.LeftSuccess);
        }

        private async Task<(SubThread, IActionResult)> IsValidSubThread(string name)
        {
            SubThread subThread = await _subthreadsService.GetByNameAsync(name);

            if (subThread is null)
                return (null, NotFound(ApiConstant.SubThread.NonExistentSubThread));
            return (subThread, null);
        }

    }
}
