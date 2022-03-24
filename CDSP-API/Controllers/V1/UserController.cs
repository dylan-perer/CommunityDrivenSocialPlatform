using CDSP_API.Contracts.V1.Requests;
using CDSP_API.Contracts.V1.Responses;
using CDSP_API.Data;
using CDSP_API.misc;
using CDSP_API.Model;
using CDSP_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace CDSP_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsersService _usersService;
        public UserController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           var users = await _usersService.GetAllAsync();

            return Ok(new UserDetailsResponse().MapToReponse(users));
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetByUsername([FromRoute] string username)
        {
            var user = await _usersService.GetByUsernameAsync(username);
            if(user == null) { return NotFound(ApiConstant.User.NonExistentUser); }

            return Ok(new UserDetailsResponse().MapToReponse(user));
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> Update([FromRoute] string username, [FromBody] UpdateUserDetailsRequest updateUserDetailsRequest)
        {
            var user = await _usersService.GetByUsernameAsync(username);
            if (user == null) return NotFound(ApiConstant.User.NonExistentUser);

            user = updateUserDetailsRequest.MapToModel(user);

            bool isSuccess = await _usersService.UpdateAsync(user);
            if (!isSuccess) return NotFound(ApiConstant.User.FailedToUpdateUser);

            return Ok(new UserDetailsResponse().MapToReponse(user));
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete([FromRoute] string username)
        {
            var user = await _usersService.GetByUsernameAsync(username);
            if (user == null) return NotFound(ApiConstant.User.NonExistentUser);

            bool isSuccess = await _usersService.DeleteAsync(user);
            if (!isSuccess) return NotFound(ApiConstant.User.FailedToUpdateUser);

            return Ok(ApiConstant.User.SuccefullyDeletedUser);
        }


    }
}
