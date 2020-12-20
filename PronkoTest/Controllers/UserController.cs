using System.Threading.Tasks;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace PronkoTest.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto userRegistration)
        {
            if (!await _userService.Register(userRegistration, ModelState))
            {
                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserAuthenticationDto userAuthentication)
        {
            if (!await _userService.Validate(userAuthentication, ModelState))
            {
                return Unauthorized();
            }

            return Ok(new {Token = _userService.CreateToken().Result});
        }
    }
}