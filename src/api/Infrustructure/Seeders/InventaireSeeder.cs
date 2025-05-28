using Data;
using domain.Entity;
using domain.Entity.Enum;

namespace Infrastructure.Seeders;

public class InventaireSeeder
{
        public static async Task<List<Inventaire>> SeedInventairesAsync(BiblioDbContext dbContext, List<Livres> livres)
    {
        var inventaires = new List<Inventaire>
        {
            new Inventaire
            {
                id_inv = Guid.NewGuid().ToString(),
                id_liv = livres[0].id_livre,
                cote_liv = "PROG-001",
                etat = etat_liv.neuf,
                statut = Statut_liv.disponible,
                inventaire = "INV-2024-001"
            },
            new Inventaire
            {
                id_inv = Guid.NewGuid().ToString(),
                id_liv = livres[1].id_livre,
                cote_liv = "DATA-001",
                etat = etat_liv.moyen,
                statut = Statut_liv.emprunté,
                inventaire = "INV-2024-002"
            },
            new Inventaire
            {
                id_inv = Guid.NewGuid().ToString(),
                id_liv = livres[2].id_livre,
                cote_liv = "ARCH-001",
                etat = etat_liv.moyen,
                statut = Statut_liv.emprunté,
                inventaire = "INV-2024-003"
            },
            new Inventaire
            {
                id_inv = Guid.NewGuid().ToString(),
                id_liv = livres[3].id_livre,
                cote_liv = "JS-001",
                etat = etat_liv.mauvais,
                statut = Statut_liv.perdu, // Livre perdu
                inventaire = "INV-2024-004"
            }
        };

        await dbContext.Inventaires.AddRangeAsync(inventaires);
        await dbContext.SaveChangesAsync();
        Console.WriteLine($"✅ Seeded {inventaires.Count} inventaires");
        return inventaires;
    }

}