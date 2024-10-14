using Microsoft.AspNetCore.Identity;

namespace ParfumeExpressApi.Helper
{
    public class RoleSeeder
    {
        public static async Task SeedRolesAndAdminUser(
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            // <summary>
            // On startup adding role "Admin" to database and assigning Shota Tevdorashvili as the first Admin
            // </summary>

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var adminUser = await userManager.FindByEmailAsync("shotatevdorashvilibusiness@gmail.com");

            if (adminUser != null) 
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }


            // OLD CODE
            //// Create an admin user if it doesn't already exist
            //var adminUser = await userManager.FindByEmailAsync("shotatevdorashvilibusiness@gmail.com");
            //if (adminUser == null)
            //{
            //    adminUser = new IdentityUser
            //    {
            //        UserName = "ShotaTevdorashvili",
            //        Email = "shotatevdorashvilibusiness@example.com"
            //    };

            //    var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");

            //    if (result.Succeeded)
            //    {
            //        await userManager.AddToRoleAsync(adminUser, "Admin");
            //    }
            //}
        }
    }
}
