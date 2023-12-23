using Core.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Services.Services.Token_Service;
using Services.Services.User_Service.DTO;

namespace Services.Services.User_Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<ApplicationUser> GetUser(string email)
            => await _userManager.FindByEmailAsync(email);

        public async Task<UserDTO> Login(LoginDTO login)
        {
            var user = await GetUser(login.Email);

            if (user is null)
                return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

            if (!result.Succeeded)
                return null;

            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
        }

        public async Task<UserDTO> Register(RegisterDTO register)
        {
            var user = await GetUser(register.Email);

            if (user is not null)
                return null;

            var newUser = new ApplicationUser
            {
                DisplayName = register.DisplayName,
                Email = register.Email,
                UserName = register.Email.Split('@')[0]
            };

            var result = await _userManager.CreateAsync(newUser, register.Password);

            if (!result.Succeeded)
                return null;

            return new UserDTO
            {
                DisplayName = newUser.DisplayName,
                Email = newUser.Email,
                Token = _tokenService.CreateToken(newUser)
            };
        }
    }
}
