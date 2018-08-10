using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace ResourceServer.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var result = new {
                Name = "ResourceServer.AspNetCore",
                UserName = User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value
            };

            return Ok(result);
        }
    }
}
