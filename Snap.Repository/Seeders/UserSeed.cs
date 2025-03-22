using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Snap.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snap.Repository.Seeders
{
    public class UserSeed
    {
        public static async Task SeedUserAsync(UserManager<User> userManager)
        {


            if (!await userManager.Users.AnyAsync())
            {

                var user1 = new User()
                {
                    DispalyName = "Marslino Ashraf",
                    UserName = "marslinoashraf45",
                    Email = "marslinoashraf45@gmail.com",
                    PhoneNumber = "1234567890"
                };
                var result = await userManager.CreateAsync(user1, "Test1!");

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }

            }
        }
    }
}
