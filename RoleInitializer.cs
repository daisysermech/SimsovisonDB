using SimsovisionDataBase.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace SimsovisionDataBase
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "faithlm64@gmail.com";
            string password = "Frostiesermech0)";

            string moderEmail = "moderator@gmail.com";
            string moderpassword = "Moderator1.";

            if (await roleManager.FindByNameAsync("admin") == null)
                await roleManager.CreateAsync(new IdentityRole("admin"));

            if (await roleManager.FindByNameAsync("moder") == null)
                await roleManager.CreateAsync(new IdentityRole("moder"));

            if (await roleManager.FindByNameAsync("user") == null)
                await roleManager.CreateAsync(new IdentityRole("user"));

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "admin");
            }

            if (await userManager.FindByNameAsync(moderEmail) == null)
            {
                User moder = new User { Email = moderEmail, UserName = moderEmail };
                IdentityResult result = await userManager.CreateAsync(moder, moderpassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(moder, "moder");
            }


        }
    }
}