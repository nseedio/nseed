using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmokeTests.BusinessModel.Services.Users;

namespace SmokeTests.Client.RestApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            return Ok((await userService.GetAllUsers()).Select(user => user.FirstName));
        }
    }
}