using Data;
using domain.Entity;

namespace Infrastructure.Seeders;

public class NouveauteSeeder
{
    
    public static async Task SeedNouveautesAsync(BiblioDbContext dbContext, string biblio1Id, string biblio2Id)
    {
        var nouveautes = new List<Nouveaute>
        {
            new Nouveaute
            {
                id_nouv = Guid.NewGuid().ToString(),
                id_biblio = biblio1Id,
                titre = "Nouvelle Collection Informatique",
                fichier = new Dictionary<string, object>
                {
                    { "type", "pdf" },
                    { "nom", "collection_informatique_2024.pdf" },
                    { "taille", "2.5MB" },
                    { "url", "https://isgs.rnu.tn/useruploads/files/3lsc%20ff.pdf.pdf" }
                },
                description = "Découvrez notre nouvelle collection de livres d'informatique pour 2024. Plus de 50 nouveaux titres disponibles !",
                date_publication = DateTime.UtcNow.AddDays(-7),
                couverture = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Favf.asso.fr%2Famboise%2Fwp-content%2Fuploads%2Fsites%2F171%2F2021%2F03%2FLogo-Nouveau.jpg&f=1&nofb=1"
            },
            new Nouveaute
            {
                id_nouv = Guid.NewGuid().ToString(),
                id_biblio = biblio2Id,
                titre = "Horaires d'été 2024",
                fichier = new Dictionary<string, object>
                {
                    { "type", "image" },
                    { "nom", "horaires_ete_2024.jpg" },
                    { "taille", "1.2MB" },
                    { "url", "https://isgs.rnu.tn/stylesheets/images/shapes-2_04.pngjpg" }
                },
                description = "Nouveaux horaires d'ouverture pour la période estivale. La bibliothèque sera ouverte de 8h à 16h du lundi au vendredi.",
                date_publication = DateTime.UtcNow.AddDays(-3),
                couverture = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Favf.asso.fr%2Famboise%2Fwp-content%2Fuploads%2Fsites%2F171%2F2021%2F03%2FLogo-Nouveau.jpg&f=1&nofb=1"
            },
            new Nouveaute
            {
                id_nouv = Guid.NewGuid().ToString(),
                id_biblio = biblio1Id,
                titre = "Atelier Formation Recherche",
                fichier = new Dictionary<string, object>
                {
                    { "type", "document" },
                    { "nom", "formation_recherche_programme.docx" },
                    { "taille", "0.8MB" },
                    { "url", @"C:\Users\salsa\Downloads\page-word.com-facture6.docx" }
                },
                description = "Inscrivez-vous à nos ateliers de formation à la recherche documentaire. Sessions tous les mardis à 14h.",
                date_publication = DateTime.UtcNow.AddDays(-1),
                couverture = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Favf.asso.fr%2Famboise%2Fwp-content%2Fuploads%2Fsites%2F171%2F2021%2F03%2FLogo-Nouveau.jpg&f=1&nofb=1"
            }
        };

        await dbContext.Nouveautes.AddRangeAsync(nouveautes);
        await dbContext.SaveChangesAsync();
        Console.WriteLine($"✅ Seeded {nouveautes.Count} nouveautes");
    }

}