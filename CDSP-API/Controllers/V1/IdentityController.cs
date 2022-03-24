using CDSP_API.Contracts.V1.Requests;
using CDSP_API.Contracts.V1.Responses;
using CDSP_API.Data;
using CDSP_API.misc;
using CDSP_API.Model;
using CDSP_API.Models;
using CDSP_API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CDSP_API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService IdentityService;
        public IdentityController(IIdentityService identityService)
        {
            IdentityService = identityService;
        }

        [HttpPost("Signin")]
        public async Task<IActionResult> Signin([FromBody]SigninRequest signinRequest)
        {
            User user = signinRequest.MapToModel();

            AuthResult authResult = await IdentityService.SigninAsync(user);
            if(authResult == null) return NotFound(ApiConstant.Identity.NonExistentIdentity);

            return Ok(authResult);
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody]SignupRequest signupRequest)
        {
            User user = signupRequest.MapToModel();

            AuthResult authResult = await IdentityService.SignupAsync(user);
            if(authResult == null) return BadRequest(ApiConstant.Identity.SignupFailed);

            return Ok(authResult);
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody]TokenRefreshRequest tokenRefreshRequest)
        {
            AuthResult authResult = await IdentityService.VerifyAndGenerateToken(tokenRefreshRequest.Token, tokenRefreshRequest.RefreshToken);
            if (authResult is null) return BadRequest(ApiConstant.GenericError);
            
            return Ok(authResult);
        }
    }
}
