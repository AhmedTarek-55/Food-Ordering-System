using Core.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Identity.Context
{
    public class AdminIdentityDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public AdminIdentityDbContext(DbContextOptions<AdminIdentityDbContext> options) : base(options)
        {
        }
    }
}
