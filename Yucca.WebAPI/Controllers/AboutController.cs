using Microsoft.AspNetCore.Mvc;
using Yucca;

namespace Yucca.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AboutController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAboutInfo()
        {
            var aboutInfo = About.GetAboutInfo();
            return Ok(aboutInfo);
        }
    }
}