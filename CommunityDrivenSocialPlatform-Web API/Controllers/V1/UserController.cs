using CDSP_API.Contracts;
using CDSP_API.Contracts.V1.Requests;
using CDSP_API.Contracts.V1.Responses;
using CDSP_API.Data;
using CDSP_API.misc;
using CDSP_API.Model;
using CDSP_API.Models;
using CDSP_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace CDSP_API.Controllers
{
    [Route(ApiRoutes.BaseAndVersionV1 + ApiRoutes.Controller.UsersController)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUsersService usersService, ILogger<UserController> logger)
        {
            _usersService = usersService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            (EnityCoreResult ecr, List<User> users) = await _usersService.GetAllAsync();
            if (ecr.ErrorMsg != null)
            {
                _logger.LogError(ecr.ToString());
            }

            return Ok(new UserDetailsResponse().MapToReponse(users));
        }

        [HttpGet(ApiRoutes.Controller.RouteVariable.Username)]
        public async Task<IActionResult> GetByUsername([FromRoute] string username)
        {
            (EnityCoreResult ecr, User user) = await _usersService.GetByUsernameAsync(username);
            if (user == null)
            {
                _logger.LogError(ecr.ToString(username));
                return NotFound(ApiConstant.User.NonExistentUser);
            }

            return Ok(new UserDetailsResponse().MapToReponse(user));
        }

        [HttpPut(ApiRoutes.Controller.RouteVariable.Username)]
        public async Task<IActionResult> Update([FromRoute] string username, [FromBody] UpdateUserDetailsRequest updateUserDetailsRequest)
        {
            (EnityCoreResult ecr, User user) = await _usersService.GetByUsernameAsync(username);
            if (user == null)
            {
                _logger.LogError(ecr.ToString(updateUserDetailsRequest));
                return NotFound(ApiConstant.User.NonExistentUser);
            }

            user = updateUserDetailsRequest.MapToModel(user);

            EnityCoreResult updateEcr = await _usersService.UpdateAsync(user);
            if (!updateEcr.IsSuccess)
            {
                _logger.LogError(updateEcr.ToString(updateUserDetailsRequest));
                return NotFound(ApiConstant.User.FailedToUpdateUser);
            }

            return Ok(new UserDetailsResponse().MapToReponse(user));
        }

        [HttpDelete(ApiRoutes.Controller.RouteVariable.Username)]
        public async Task<IActionResult> Delete([FromRoute] string username)
        {
            (EnityCoreResult ecr, User user) = await _usersService.GetByUsernameAsync(username);
            if (user == null) return NotFound(ApiConstant.User.NonExistentUser);

            EnityCoreResult deleteEcr = await _usersService.DeleteAsync(user);
            if (!deleteEcr.IsSuccess)
            {
                _logger.LogError(ecr.ToString(username));
                return NotFound(ApiConstant.User.FailedToUpdateUser);
            }
            return Ok(ApiConstant.User.SuccefullyDeletedUser);
        }


    }
}
