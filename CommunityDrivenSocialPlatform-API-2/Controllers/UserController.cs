using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommunityDrivenSocialPlatform_API_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUser()
        {
            return Ok(new { messgae = "ok"});
        }
    }
}
