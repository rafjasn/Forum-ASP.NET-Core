using AspNetCoreMvcIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcIdentity.Data
{
    public class DataSeed
    {
        private ApplicationDbContext _context;

        public DataSeed(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
        public  Task SeedSuperUser()
        {
            var roleStore = new RoleStore<IdentityRole>(_context);


            var userStore = new UserStore<ApplicationUser>(_context);


            var user = new ApplicationUser
            {
                UserName = "Admin",
                NormalizedUserName = "admin",
                Email = "admin@email.com",
                NormalizedEmail = "example@email.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),

            };

            var hasher = new PasswordHasher<ApplicationUser>();
            var hashedPassword = hasher.HashPassword(user, "admin");

            user.PasswordHash = hashedPassword;

            var hasAdminRole = _context.Roles.Any(r => r.Name == "Admin");

            if(!hasAdminRole)
            {
                roleStore.CreateAsync(new IdentityRole
              { Name = "Admin", NormalizedName = "admin" });
            }

            var hasSuperUser = _context.Users
                .Any(u => u.NormalizedUserName == u.UserName);

            if(!hasSuperUser)
            {
                 userStore.CreateAsync(user);
                 userStore.AddToRoleAsync(user, "Admin");
            }

             _context.SaveChangesAsync();
            return Task.CompletedTask;
        }
        */
    }
}
