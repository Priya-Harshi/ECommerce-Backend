using System;
using System.Collections.Generic;
using System.Text;
using Ecommerce.Data.Context;
using Ecommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Business.Services
{
    public class AdminSeeder
    {
        public static async Task SeedAsync(ECommerceDbContext context)
        {
            var adminExists = context.Users
                .Any(u => u.Role == "Admin");

            if (adminExists)
            {
                return;
            }

            var admin = new User
            {
                Name = "Admin",
                Email = "admin@ecommerce.com",
                Role = "Admin"
            };

            var passwordHasher = new PasswordHasher<User>();

            admin.PasswordHash =
                passwordHasher.HashPassword(
                    admin,
                    "Admin@123"
                );

            context.Users.Add(admin);

            await context.SaveChangesAsync();
        }
    }
}
