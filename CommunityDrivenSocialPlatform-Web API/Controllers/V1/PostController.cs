using CDSP_API.Contracts;
using CDSP_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CDSP_API.Controllers.V1
{
    [Route(ApiRoutes.BaseAndVersionV1 + ApiRoutes.Controller.IdentityController)]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<IdentityController> _logger;
        public PostController(IIdentityService identityService, ILogger<IdentityController> logger)
        {
            _logger = logger;
            _identityService = identityService;
        }

    }
}