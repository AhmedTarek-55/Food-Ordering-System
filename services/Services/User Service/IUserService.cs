using Core.Identity.Entities;
using Services.Services.User_Service.DTO;

namespace Services.Services.User_Service
{
    public interface IUserService
    {
        Task<UserDTO> Register(RegisterDTO register);
        Task<UserDTO> Login(LoginDTO login);
        Task<ApplicationUser> GetUser(string email);
    }
}
