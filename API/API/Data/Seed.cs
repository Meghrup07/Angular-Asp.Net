using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
	public class Seed
	{
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

            foreach (var user in users)
            {
                if (user.passwordHash == null || user.passwordSalt == null)
                {
                    using var hmac = new HMACSHA512();
                    user.passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("P@assw0rd"));
                    user.passwordSalt = hmac.Key;
                }

                user.UserName = user.UserName.ToLower();
                context.Users.Add(user);
            }

            await context.SaveChangesAsync();
        }

    }
}

