using Microsoft.AspNetCore.Identity;
using OnlineSupermarket.Data;
using OnlineSupermarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSupermarket.Services
{
    public class Initializer
    {
        private ApplicationDbContext _context;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<IdentityUser> _userManager;

        public Initializer(
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            if (!_context.Roles.Any(r => r.Name == "Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }

            if (!_context.Roles.Any(r => r.Name == "User"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "User" });
            }

            if (!_context.Users.Any(u => u.UserName == "admin@user.com"))
            {
                var user = new ApplicationUser
                {
                    Email = "admin@user.com",
                    UserName = "admin@user.com"
                };
                await _userManager.CreateAsync(user, "Pass123!");
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
