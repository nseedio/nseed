using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using SmokeTests.Persistence.Repositories.EntityFramework;

namespace SmokeTests.Client.RestApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        [Route]
        [HttpGet]
        public Task<IEnumerable<string>> Get()
        {
            var dbContext = new SmokeTestsDbContext();
            return Task.FromResult<IEnumerable<string>>(dbContext.Users.Select(user => user.FirstName));
        }
    }
}
