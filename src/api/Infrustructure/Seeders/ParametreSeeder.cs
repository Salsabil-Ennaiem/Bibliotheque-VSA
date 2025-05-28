using Data;
using domain.Entity;

namespace Infrastructure.Seeders;

public class parametreSeeder
{
        public static async Task<(Parametre ancien, Parametre nouveau)> SeedParametresAsync(BiblioDbContext dbContext, string biblio1Id, string biblio2Id)
    {
        var ancienParametre = new Parametre
        {
            id_param = Guid.NewGuid().ToString(),
            IdBiblio = biblio1Id,
            Delais_Emprunt_Etudiant = 7,
            Delais_Emprunt_Enseignant = 10,
            Delais_Emprunt_Autre = 5,
            Modele_Email_Retard = "Rappel: Votre livre [TITRE] est en retard depuis le [DATE]",
            date_modification = DateTime.UtcNow.AddDays(-30) // Il y a 30 jours
        };

        var nouveauParametre = new Parametre
        {
            id_param = Guid.NewGuid().ToString(),
            IdBiblio = biblio1Id,
            Delais_Emprunt_Etudiant = 14,
            Delais_Emprunt_Enseignant = 20,
            Delais_Emprunt_Autre = 10,
            Modele_Email_Retard = "URGENT: Votre livre [TITRE] est en retard depuis le [DATE]. Merci de le retourner rapidement.",
            date_modification = DateTime.UtcNow.AddDays(-10) // Il y a 10 jours
        };

        await dbContext.Parametres.AddRangeAsync(new[] { ancienParametre, nouveauParametre });
        await dbContext.SaveChangesAsync();
        Console.WriteLine($"✅ Seeded 2 parametres with different dates");
        return (ancienParametre, nouveauParametre);
    }

}