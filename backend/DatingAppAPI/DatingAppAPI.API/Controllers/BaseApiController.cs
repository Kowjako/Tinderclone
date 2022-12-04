using DatingAppAPI.API.Filter;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LogUserActivityFilter))]
    public class BaseApiController : ControllerBase { }
}
