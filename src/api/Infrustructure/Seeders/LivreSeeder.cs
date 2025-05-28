using Data;
using domain.Entity;

namespace Infrastructure.Seeders;

public class LivreSeeder
{
    
    public static async Task<List<Livres>> SeedLivresAsync(BiblioDbContext dbContext, string biblio1Id, string biblio2Id)
    {
        var livres = new List<Livres>
        {
            new Livres
            {
                id_livre = Guid.NewGuid().ToString(),
                id_biblio = biblio1Id,
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
                id_biblio = biblio1Id,
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
                id_biblio = biblio2Id,
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
                id_biblio = biblio2Id,
                date_edition = "2023",
                titre = "JavaScript Avancé",
                auteur = "Sophie Bernard",
                isbn = "978-2-3333-4444-5",
                editeur = "Editions JS",
                Description = "JavaScript pour experts",
                Langue = "Français" ,
                couverture ="https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fmedia.s-bol.com%2FgvNWrkL4p0j%2F550x778.jpg&f=1&nofb=1&ipt=356c8656df7f36264c9b4360fdcca8ef141f447066a0d2fee0bdb8876b137a87"
            }
        };

        await dbContext.Livres.AddRangeAsync(livres);
        await dbContext.SaveChangesAsync();
        Console.WriteLine($"✅ Seeded {livres.Count} books");
        return livres;
    }

}