using DatingAppAPI.Persistance.Data;
using DatingAppAPI.Persistance.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppAPI.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _dbContext;

        public BuggyController(DataContext context)
        {
            _dbContext = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _dbContext.Users.Find(-1);
            if(thing == null)
            {
                return NotFound();
            }
            return Ok(thing);
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _dbContext.Users.Find(-1);
            var thingToReturn = thing.ToString();

            return thingToReturn;
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("Bad req");
        }
    }
}
