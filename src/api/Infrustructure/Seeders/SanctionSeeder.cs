using domain.Entity;
using domain.Entity.Enum;
using Data;

namespace Infrustructure.Seeders;

public class SanctionSeeder
{

    public static async Task SeedSanctionsAsync(BiblioDbContext dbContext, List<Emprunts> emprunts, List<Membre> membres, string biblio1Id)
    {
        var sanctions = new List<Sanction>
        {
            // Sanction pour Fatma (emprunt en retard)
            new Sanction
            {
                id_sanc = Guid.NewGuid().ToString(),
                id_membre = membres[1].id_membre,
                id_biblio = biblio1Id,
                id_emp = emprunts[1].id_emp,
                raison = Raison_sanction.retard,
                date_sanction = DateTime.UtcNow.AddDays(-20),
                date_fin_sanction = DateTime.UtcNow.AddDays(10), // Sanction de 30 jours
                montant = 15.00m,
                payement = false,
                active = true,
                description = "Retard de 10 jours pour le livre Architecture Web"
            },
            // Sanction pour Mohamed (livre perdu)
            new Sanction
            {
                id_sanc = Guid.NewGuid().ToString(),
                id_membre = membres[2].id_membre,
                id_biblio = biblio1Id,
                id_emp = emprunts[2].id_emp,
                raison = Raison_sanction.perte,
                date_sanction = DateTime.UtcNow.AddDays(-100),
                date_fin_sanction = null, // Pas de fin tant que pas retourne le livre ou payer 
                montant = 45.00m, // Amende de 45dt pour livre perdu
                payement = false,
                active = true,
                description = "Livre JavaScript Avancé perdu depuis plus d'1 an"
            },
            // Sanction pour Ahmed (deuxième emprunt en retard)
            new Sanction
            {
                id_sanc = Guid.NewGuid().ToString(),
                id_membre = membres[0].id_membre, // Ahmed
                id_biblio = biblio1Id,
                id_emp = emprunts[3].id_emp,
                raison = Raison_sanction.retard,
                date_sanction = DateTime.UtcNow.AddDays(-2),
                date_fin_sanction = DateTime.UtcNow.AddDays(5), // Sanction de 7 jours
                montant = 8.00m,
                payement = true,
                active = true,
                description = "Retard de 1 jour pour le livre Programmation C# - payé"
            }
        };

        await dbContext.Sanctions.AddRangeAsync(sanctions);
        await dbContext.SaveChangesAsync();
        Console.WriteLine($"✅ Seeded {sanctions.Count} sanctions");
    }

}