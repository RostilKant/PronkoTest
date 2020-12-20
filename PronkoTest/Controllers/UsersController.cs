using System.Security.Claims;
using System.Threading.Tasks;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace PronkoTest.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
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

        [HttpGet("profile"), Authorize]
        public async Task<IActionResult> GetUserProfile() => 
            Ok(await _userService.GetInformation(HttpContext.User.Identity?.Name));

        [HttpPut("edit-profile"), Authorize]
        public async Task<IActionResult> EditUserProfile([FromBody] UserUpdateDto userUpdate)
        {
            await _userService.EditInformation(HttpContext.User.Identity?.Name, userUpdate);
            return NoContent();
        }

        [HttpPut("edit-profile/change-pass"), Authorize]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePassDto changePass)
        {
            await _userService.ChangePassword(HttpContext.User.Identity?.Name, changePass, ModelState);
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
        

    }
}