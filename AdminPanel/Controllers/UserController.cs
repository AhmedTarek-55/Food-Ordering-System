using AdminPanel.Models.RoleViewModels;
using AdminPanel.Models.UserViewModels;
using Core.Identity.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var AppUsers = await _userManager.Users.ToListAsync();
            var Users = AppUsers.Select(user => new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Roles = _userManager.GetRolesAsync(user).Result
            }).ToList();

            return View(Users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _roleManager.Roles.ToListAsync();

            var userRolesVM = new UserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select( r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList()
            };

            return View(userRolesVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit (UserRoleViewModel userRolesVM, string id)
        {
            if (id != userRolesVM.UserId)
                return NotFound();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userRolesVM.UserId);

                var userRoles = await _userManager.GetRolesAsync(user);

                foreach (var role in userRolesVM.Roles)
                {
                    if (userRoles.Any(r => r == role.Name) && !role.IsSelected)
                        await _userManager.RemoveFromRoleAsync(user, role.Name);

                    if (!userRoles.Any(r => r == role.Name) && role.IsSelected)
                        await _userManager.AddToRoleAsync(user, role.Name);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userRolesVM);
        }
    }
}
