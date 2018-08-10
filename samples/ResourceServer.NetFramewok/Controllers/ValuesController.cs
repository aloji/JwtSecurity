using System.Web.Http;

namespace ResourceServer.NetFramewok.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        [HttpGet]
        public string Index()
        {
            return "ResourceServer.NetFramewok";
        }
    }
}
