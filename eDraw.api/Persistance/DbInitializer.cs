using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace eDraw.api.Persistance
{
    public static class DbInitializer
    {


        public static void Initialize(EDrawDbContext context)
        {
            context.Database.EnsureCreated();
            //if (context.Status.Any())
            //{
            //    /* return; */ // DB has been seeded
            //}
        }

        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            //adding customs roles
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = {"Bank", "HomeOwner", "GeneralContractor", "SubContractor"};

            foreach (var roleName in roleNames)
            {
                //creating the roles and seeding them to the database
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}

