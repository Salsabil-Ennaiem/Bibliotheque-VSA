using Data;
using domain.Entity;
using Domaine.Entity;

namespace Infrastructure.Seeders;

public class NouveauteSeeder
{
    
    public static async Task SeedNouveautesAsync(BiblioDbContext dbContext, string biblio1Id, string biblio2Id)

{
    // Create fichiers for first Nouveaute
    var fichiers1 = new List<Fichier>
    {
        new Fichier
        {
            IdFichier = Guid.NewGuid().ToString(),
            NomFichier = "collection_informatique_2024.pdf",
            TypeFichier = "pdf",
            TailleFichier = 2_500_000, // 2.5 MB approx
            CheminFichier = "https://isgs.rnu.tn/useruploads/files/3lsc%20ff.pdf.pdf",
            DateCreation = DateTime.UtcNow,
            ContenuFichier = Array.Empty<byte>() // or load actual bytes
        }
    };

    var couverture1 = new Fichier
    {
        IdFichier = Guid.NewGuid().ToString(),
        NomFichier = "cover_image.jpg",
        TypeFichier = "image/jpeg",
        TailleFichier = 100_000,
        CheminFichier = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Favf.asso.fr%2Famboise%2Fwp-content%2Fuploads%2Fsites%2F171%2F2021%2F03%2FLogo-Nouveau.jpg&f=1&nofb=1",
        DateCreation = DateTime.UtcNow,
        ContenuFichier = Array.Empty<byte>()
    };

    var nouveaute1 = new Nouveaute
    {
        id_nouv = Guid.NewGuid().ToString(),
        id_biblio = biblio1Id,
        titre = "Nouvelle Collection Informatique",
        description = "Découvrez notre nouvelle collection de livres d'informatique pour 2024. Plus de 50 nouveaux titres disponibles !",
        date_publication = DateTime.UtcNow.AddDays(-7),
        couverture = couverture1.IdFichier,
        Couvertures = couverture1,
        Fichiers = fichiers1
    };

    // Link fichiers to nouveaute1
    foreach (var f in fichiers1)
    {
        f.NouveauteId = nouveaute1.id_nouv;
        f.ficherNouv = nouveaute1;
    }
    couverture1.couvertureNouv = nouveaute1;

    // Repeat similar for other nouveautes...

    // Example for second Nouveaute (simplified)
    var couverture2 = new Fichier
    {
        IdFichier = Guid.NewGuid().ToString(),
        NomFichier = "cover_image2.jpg",
        TypeFichier = "image/jpeg",
        TailleFichier = 100_000,
        CheminFichier = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Favf.asso.fr%2Famboise%2Fwp-content%2Fuploads%2Fsites%2F171%2F2021%2F03%2FLogo-Nouveau.jpg&f=1&nofb=1",
        DateCreation = DateTime.UtcNow,
        ContenuFichier = Array.Empty<byte>()
    };

    var nouveaute2 = new Nouveaute
    {
        id_nouv = Guid.NewGuid().ToString(),
        id_biblio = biblio2Id,
        titre = "Horaires d'été 2024",
        description = "Nouveaux horaires d'ouverture pour la période estivale. La bibliothèque sera ouverte de 8h à 16h du lundi au vendredi.",
        date_publication = DateTime.UtcNow.AddDays(-3),
        couverture = couverture2.IdFichier,
        Couvertures = couverture2,
        Fichiers = new List<Fichier>() // no additional files in this example
    };
    couverture2.couvertureNouv = nouveaute2;

    // Add all fichiers and nouveautes to context
    await dbContext.Fichiers.AddRangeAsync(fichiers1);
    await dbContext.Fichiers.AddAsync(couverture1);
    await dbContext.Fichiers.AddAsync(couverture2);

    await dbContext.Nouveautes.AddAsync(nouveaute1);
    await dbContext.Nouveautes.AddAsync(nouveaute2);

    await dbContext.SaveChangesAsync();

    Console.WriteLine("✅ Seeded nouveautes with fichiers and covers");
}

}