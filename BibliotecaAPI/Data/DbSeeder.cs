using BibliotecaAPI.Auth;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Data
{
    public class DbSeeder
    {
        public static async Task SeedAsync(BibliotecaContext context, PasswordHasher passwordHasher) {
            await context.Database.MigrateAsync();

            if (await context.UsersSystem.AnyAsync()) return;

            var admin = new UserSystem
            {
                Username = "admin",
                PasswordHash = passwordHasher.Hash("Admin123!"),
                Role = "admin"
            };

            context.UsersSystem.Add(admin);

            await context.SaveChangesAsync();
        }
    }
}
