using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommunityDrivenSocialPlatform_APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        [Authorize(Roles = "USER, ADMIN")]
        [HttpGet]
        public IActionResult Feed()
        {
            return Ok("WELCOME LOGGED USER");
        }


    }
}
