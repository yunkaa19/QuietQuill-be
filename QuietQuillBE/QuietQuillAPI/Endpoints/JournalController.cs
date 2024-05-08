using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuietQuillBE.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalController : ControllerBase
    {
        
        [HttpGet, Authorize]
        public IActionResult Get()
        {
            return Ok("Hello World");
        }
    }
}
