using Data;
using domain.Entity;

namespace Infrastructure.Seeders;

public class LivreSeeder
{
    
    public static async Task<List<Livres>> SeedLivresAsync(BiblioDbContext dbContext)
    {
        var livres = new List<Livres>
        {
            new Livres
            {
                id_livre = Guid.NewGuid().ToString(),
                date_edition = "2023",
                titre = "Programmation C#",
                auteur = "Jean Martin",
                isbn = "978-2-1234-5678-9",
                editeur = "Editions Tech",
                Description = "Guide complet C#",
                Langue = "Français"
            },
            new Livres
            {
                id_livre = Guid.NewGuid().ToString(),
                date_edition = "2022",
                titre = "Base de Données",
                auteur = "Marie Durand",
                isbn = "978-2-9876-5432-1",
                editeur = "Editions Data",
                Description = "Bases de données relationnelles",
                Langue = "Français"
            },
            new Livres
            {
                id_livre = Guid.NewGuid().ToString(),
                date_edition = "2024",
                titre = "Architecture Web",
                auteur = "Pierre Blanc",
                isbn = "978-2-5555-7777-3",
                editeur = "Editions Web",
                Description = "Architecture moderne",
                Langue = "Français"
            },
            new Livres
            {
                id_livre = Guid.NewGuid().ToString(),
                date_edition = "2023",
                titre = "JavaScript Avancé",
                auteur = "Sophie Bernard",
                isbn = "978-2-3333-4444-5",
                editeur = "Editions JS",
                Description = "JavaScript pour experts",
                Langue = "Français" 
            }
        };

        await dbContext.Livres.AddRangeAsync(livres);
        await dbContext.SaveChangesAsync();
        Console.WriteLine($"✅ Seeded {livres.Count} books");
        return livres;
    }

}