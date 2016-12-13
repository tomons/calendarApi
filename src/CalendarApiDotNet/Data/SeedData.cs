using CalendarApiDotNet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarApiDotNet.Data
{
    public static class SeedData
    {
        public const string AdminRole = "Administrator";

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.EnsureCreated();
                CreateUsersAndRoles(serviceProvider, context).Wait();
                context.SaveChanges();          
            }
        }

        private static async Task CreateUsersAndRoles(IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            //var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            
            string[] roles = new string[] { AdminRole };

            foreach (string role in roles)
            {
                if (!context.Roles.Any(r => r.Name == role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }            

            const string adminUserName = "tomons@gmail.com";

            var adminUser = new ApplicationUser
            {
                Email = "tomons@gmail.com",
                NormalizedEmail = "TOMONS@GMAIL.COM",
                UserName = adminUserName,
                NormalizedUserName = adminUserName.ToUpper(),
                PhoneNumber = "+923366633352",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            
            if (!context.Users.Any(u => u.UserName == adminUserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(adminUser, "Admin123");
                adminUser.PasswordHash = hashed;                
                await userStore.CreateAsync(adminUser);                                              
            }

            //adminUser = await userManager.FindByEmailAsync("tomons@gmail.com");
            if (!await userStore.IsInRoleAsync(adminUser, AdminRole))
            {
                var user = await userManager.FindByNameAsync(adminUserName);
                await userManager.AddToRoleAsync(user, AdminRole);                
            }            
        }
    }
}
