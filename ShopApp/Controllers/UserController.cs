using Microsoft.AspNetCore.Mvc;
using Shop.Domain.Models;

namespace ShopApp.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            return Ok(user);
        }
    }
}
