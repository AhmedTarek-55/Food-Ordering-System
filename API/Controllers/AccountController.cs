using API.Handle_Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.User_Service;
using Services.Services.User_Service.DTO;
using System.Security.Claims;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO register)
        {
            var user = await _userService.Register(register);

            if (user == null)
                return BadRequest(new ApiException(400, "Email Already Exist"));

            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO login)
        {
            var user = await _userService.Login(login);

            if (user == null)
                return Unauthorized(new ApiResponse(401));

            return Ok(user);
        }

        [HttpGet("GetCurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);

            var user = await _userService.GetUser(email);

            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Email = user.Email
            };
        }

    }
}
