using Core.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helper
{
    public class AppIdentityContextSeed
    {
        public static async Task UserSeedAsync (UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any ()) 
            {
                var user = new ApplicationUser
                {
                    DisplayName = "Jack",
                    Email = "JackCooper@gmail.com",
                    UserName = "JackCooper22",
                    Address = new Address
                    {
                        FirstName = "Jack",
                        LastName = "Cooper",
                        Street = "5308 Flower Cir",
                        State = "CO",
                        City = "Arvada",
                        ZipCode = "80002"
                    }
                };

                await userManager.CreateAsync (user, "Password123!");
            }
        }
    }
}
