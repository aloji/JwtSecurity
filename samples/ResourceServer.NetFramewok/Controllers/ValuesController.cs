using System.Security.Claims;
using System.Web.Http;
using System.Linq;

namespace ResourceServer.NetFramewok.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var identity = User?.Identity as ClaimsIdentity;
            var result = new
            {
                Name = "ResourceServer.NetFramewok",
                UserName = identity?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value
            };

            return Ok(result);
        }
    }
}
