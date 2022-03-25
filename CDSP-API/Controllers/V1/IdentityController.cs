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
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CDSP_API.Controllers.V1
{
    [Route("api/v1/[controller]")]
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

        [HttpPost("Signin")]
        public async Task<IActionResult> Signin([FromBody] SigninRequest signinRequest)
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

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest signupRequest)
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

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRefreshRequest tokenRefreshRequest)
        {
            AuthResult authResult = await _identityService.VerifyAndGenerateToken(tokenRefreshRequest.Token, tokenRefreshRequest.RefreshToken);
            if (authResult is null) return BadRequest(ApiConstant.GenericError);

            return Ok(authResult);
        }
    }
}
