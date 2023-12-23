using Core.Context;
using Core.Identity.Context;
using Core.Identity.Entities;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Helper
{
    public class ApplySeeding
    {
        public static async Task ApplySeedingAsync (WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var context = services.GetRequiredService<StoreDbContext>();

                    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                    var userManeger = services.GetRequiredService<UserManager<ApplicationUser>>();

                    await context.Database.MigrateAsync();

                    await StoreContextSeed.SeedAsync(context, loggerFactory);
                    await AppIdentityContextSeed.UserSeedAsync(userManeger);
                }
                catch (Exception ex) 
                {
                    var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                    logger.LogError(ex.Message);
                }
            }
        }
    }
}
