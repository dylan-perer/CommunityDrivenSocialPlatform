using CDSP_API.Contracts;
using CDSP_API.Contracts.V1.Requests;
using CDSP_API.Data;
using CDSP_API.misc;
using CDSP_API.Models;
using CDSP_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CDSP_API.Controllers.V1
{
    [Route(ApiRoutes.BaseAndVersionV1 + ApiRoutes.Controller.IdentityController)]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<IdentityController> _logger;
        public IdentityController(IIdentityService identityService, ILogger<IdentityController> logger)
        {
            _logger = logger;
            _identityService = identityService;

        }

        [HttpPost(ApiRoutes.Controller.Action.Signin)]
        public async Task<IActionResult> SigninAsync([FromBody] SigninRequest signinRequest)
        {
            User user = signinRequest.MapToModel();

            (EnityCoreResult ecr, AuthResult authResult) = await _identityService.SigninAsync(user);
            if (authResult == null)
            { 
                _logger.LogError(ecr.ToString(signinRequest.ToString()));
                return NotFound(ApiConstant.Identity.NonExistentIdentity); 
            }

            return Ok(authResult);
        }

        [HttpPost(ApiRoutes.Controller.Action.Signup)]
        public async Task<IActionResult> SignupAsync([FromBody] SignupRequest signupRequest)
        {
            User user = signupRequest.MapToModel();

            (EnityCoreResult ecr, AuthResult authResult) = await _identityService.SignupAsync(user);

            if (!ecr.IsSuccess)
            {
                _logger.LogError(ecr.ToString(signupRequest.ToString()));
                return BadRequest(ApiConstant.Identity.SignupFailed);
            }

            return Ok(authResult);
        }

        [HttpPost(ApiRoutes.Controller.Action.RefreshToken)]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] TokenRefreshRequest tokenRefreshRequest)
        {
            AuthResult authResult = await _identityService.VerifyAndGenerateToken(tokenRefreshRequest.Token, tokenRefreshRequest.RefreshToken);
            if (authResult is null) return BadRequest(ApiConstant.GenericError);

            return Ok(authResult);
        }
    }
}
