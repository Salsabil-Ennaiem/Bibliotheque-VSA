
using domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Seeders;

public class UserSeeder
{
    public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
    {
        try
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<Bibliothecaire>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Check if roles exist
            var roleExists = await roleManager.RoleExistsAsync("ADMIN") && await roleManager.RoleExistsAsync("Bibliothecaire");
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole("Bibliothecaire"));
                await roleManager.CreateAsync(new IdentityRole("ADMIN"));
                Console.WriteLine("✅ Created 'Bibliothecaire , ADMIN' roles");
            }
            else
            {
                Console.WriteLine("ℹ️ Roles already exist");
            }

            var anyUsers = await userManager.Users.AnyAsync();
            if (anyUsers)
            {
                Console.WriteLine("ℹ️ Users already exist in database");
                return;
            }

            var users = new List<(string email, string password, string nom, string prenom)>
            {
                ("admin@example.com", "Admin@123", "Admin", "User"),
                ("biblio@example.com", "Biblio@123", "Marie", "Dupont")
            };

            var createdUserIds = new List<string>();

            foreach (var (email, password, nom, prenom) in users)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    var newUser = new Bibliothecaire
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        nom = nom,
                        prenom = prenom
                    };

                    var result = await userManager.CreateAsync(newUser, password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newUser, "Bibliothecaire");
                        Console.WriteLine($"✅ Seeded user {email}");
                        createdUserIds.Add(newUser.Id);
                    }
                }
                else
                {
                    Console.WriteLine($"ℹ️ User {email} already exists.");
                    createdUserIds.Add(user.Id);
                }
            }

            if (createdUserIds.Count >= 2)
            {
                await DataSeeder.SeedAllDataAsync(serviceProvider, createdUserIds[0], createdUserIds[1]);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error seeding users: {ex.Message}");
        }
    }
}
