using Core.Identity.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Services.User_Service.DTO;
using System.Security.Claims;

namespace AdminPanel.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login(string? ReturnUrl)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO login, string? ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(login.Email);

                if (user == null)
                {
                    ModelState.AddModelError("Email", "Email is not found, check your Email and try again");
                    return View(login);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

                if (!result.Succeeded || !await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    ModelState.AddModelError("Password", "Login Attempt Faild, You are not authorized");
                    return View(login);
                }

                if (result.Succeeded)
                {
                    var claims = new List<Claim> {
                        new Claim(ClaimTypes.Role, "Admin")
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(login);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
