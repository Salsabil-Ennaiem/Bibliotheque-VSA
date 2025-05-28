using Data;
using domain.Entity;
using domain.Entity.Enum;
namespace Infrastructure.Seeders;

public class MembreSeeder
{
    
    public static async Task<List<Membre>> SeedMembresAsync(BiblioDbContext dbContext, string biblio1Id, string biblio2Id)
    {
        var membres = new List<Membre>
        {
            new Membre
            {
                id_membre = Guid.NewGuid().ToString(),
                id_biblio = biblio1Id,
                TypeMembre = TypeMemb.Etudiant,
                nom = "Benali",
                prenom = "Ahmed",
                email = "ahmed.benali@email.com",
                telephone = "+216 20 123 456",
                cin_ou_passeport = "12345678",
                date_inscription = DateTime.UtcNow.AddMonths(-6),
                Statut = StatutMemb.actif
            },
            new Membre
            {
                id_membre = Guid.NewGuid().ToString(),
                id_biblio = biblio2Id,
                TypeMembre = TypeMemb.Enseignant,
                nom = "Trabelsi",
                prenom = "Fatma",
                email = "fatma.trabelsi@email.com",
                telephone = "+216 25 987 654",
                cin_ou_passeport = "87654321",
                date_inscription = DateTime.UtcNow.AddMonths(-3),
                Statut = StatutMemb.actif
            },
            new Membre
            {
                id_membre = Guid.NewGuid().ToString(),
                id_biblio = biblio1Id,
                TypeMembre = TypeMemb.Autre,
                nom = "Khelifi",
                prenom = "Mohamed",
                email = "mohamed.khelifi@email.com",
                telephone = "+216 22 555 777",
                cin_ou_passeport = "11223344",
                date_inscription = DateTime.UtcNow.AddMonths(-1),
                Statut = StatutMemb.sanctionne
            }
        };

        await dbContext.Membres.AddRangeAsync(membres);
        await dbContext.SaveChangesAsync();
        Console.WriteLine($"✅ Seeded {membres.Count} membres");
        return membres;
    }

}