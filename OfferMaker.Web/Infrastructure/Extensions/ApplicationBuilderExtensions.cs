using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OfferMaker.Data;
using OfferMaker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfferMaker.Web.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<OfferMakerDbContext>().Database.Migrate();

                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                Task
                    .Run(async () =>
                    {
                        var adminName = WebConstants.AdministratorRole;

                        var roles = new[]
                        {
                            adminName,
                            WebConstants.AccountManagerRole
                        };

                        foreach (var role in roles)
                        {
                            var roleExists = await roleManager.RoleExistsAsync(role);

                            if (!roleExists)
                            {
                                await roleManager.CreateAsync(new IdentityRole(role));
                            }
                        }

                        var adminEmail = "admin@gmail.com";

                        var adminUser = await userManager.FindByEmailAsync(adminEmail);

                        if (adminUser == null)
                        {

                            adminUser = new User
                            {
                                Email = adminEmail,
                                UserName = "Admin"
                            };

                            await userManager.CreateAsync(adminUser, "P@nair123");

                            await userManager.AddToRoleAsync(adminUser, adminName);
                        }
                    })
                    .Wait();
            }

            return app;
        }
    }
}
