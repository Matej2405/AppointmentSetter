using Backend.Entities;
using Microsoft.AspNetCore.Identity;

namespace Backend.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            // 1. Ensure roles exist
            var roles = new[] { "Admin", "User", "Doctor" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }

            // 2. Create admin user if it doesn't exist
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new User
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
            else
            {
                var rolesForAdmin = await userManager.GetRolesAsync(adminUser);
                if (!rolesForAdmin.Contains("Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            if (adminUser != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                var resetResult = await userManager.ResetPasswordAsync(adminUser, token, "Admin123!");

                if (resetResult.Succeeded)
                {
                    Console.WriteLine("✅ Admin password reset to Admin123!");
                }
                else
                {
                    Console.WriteLine("❌ Failed to reset admin password:");
                    foreach (var err in resetResult.Errors)
                        Console.WriteLine(err.Description);
                }
            }

        }
    }
}
