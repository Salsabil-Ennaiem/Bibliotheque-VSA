using Data;
using domain.Entity;
using domain.Entity.Enum;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seeders;

public class EmpruntsSeeder
{
    
public static async Task<List<Emprunts>> SeedEmpruntsAsync(BiblioDbContext dbContext, List<Membre> membres, List<Inventaire> inventaires, string biblio1Id, string biblio2Id)
{
    // Verify that all referenced entities exist in the database
    var existingMembres = await dbContext.Membres.Select(m => m.id_membre).ToListAsync();
    var existingInventaires = await dbContext.Inventaires.Select(i => i.id_inv).ToListAsync();
    var existingBiblios = await dbContext.Users.Select(u => u.Id).ToListAsync();

    Console.WriteLine($"🔍 Debug - Membres in DB: {existingMembres.Count}");
    Console.WriteLine($"🔍 Debug - Inventaires in DB: {existingInventaires.Count}");
    Console.WriteLine($"🔍 Debug - Biblios in DB: {existingBiblios.Count}");

    // Print actual IDs for debugging
    Console.WriteLine($"🔍 Debug - Membre IDs: {string.Join(", ", existingMembres)}");
    Console.WriteLine($"🔍 Debug - Inventaire IDs: {string.Join(", ", existingInventaires)}");
    Console.WriteLine($"🔍 Debug - Biblio IDs: {string.Join(", ", existingBiblios)}");

    var emprunts = new List<Emprunts>
    {
        // Emprunt normal - Ahmed
        new Emprunts
        {
            id_emp = Guid.NewGuid().ToString(),
            id_membre = membres[0].id_membre,
            id_biblio = biblio1Id,
            Id_inv = inventaires[1].id_inv, // Make sure this matches exactly
            date_emp = DateTime.UtcNow.AddDays(-25),
            date_retour_prevu = DateTime.UtcNow.AddDays(-18),
            date_effectif = DateTime.UtcNow.AddDays(-20),
            Statut_emp = Statut_emp.retourne,
            note = "Emprunt retourné à temps"
        },
        // Emprunt en retard - Fatma (EN_COURS mais date dépassée)
        new Emprunts
        {
            id_emp = Guid.NewGuid().ToString(),
            id_membre = membres[1].id_membre,
            id_biblio = biblio2Id,
            Id_inv = inventaires[2].id_inv,
            date_emp = DateTime.UtcNow.AddDays(-35),
            date_retour_prevu = DateTime.UtcNow.AddDays(-25),
            date_effectif = null,
            Statut_emp = Statut_emp.en_cours,
            note = "Emprunt en retard - sanction appliquée"
        },
        // Emprunt perdu - Mohamed (plus de 1 an)
        new Emprunts
        {
            id_emp = Guid.NewGuid().ToString(),
            id_membre = membres[2].id_membre,
            id_biblio = biblio1Id,
            Id_inv = inventaires[3].id_inv,
            date_emp = DateTime.UtcNow.AddDays(-400),
            date_retour_prevu = DateTime.UtcNow.AddDays(-385),
            date_effectif = null,
            Statut_emp = Statut_emp.perdu,
            note = "Livre perdu - emprunt dépassé 1 an"
        },
        // Deuxième emprunt Ahmed (pour avoir au moins 1 emprunt par membre)
        new Emprunts
        {
            id_emp = Guid.NewGuid().ToString(),
            id_membre = membres[0].id_membre,
            id_biblio = biblio1Id,
            Id_inv = inventaires[0].id_inv,
            date_emp = DateTime.UtcNow.AddDays(-15),
            date_retour_prevu = DateTime.UtcNow.AddDays(-1),
            date_effectif = null,
            Statut_emp = Statut_emp.en_cours,
            note = "Deuxième emprunt Ahmed - en retard"
        }
    };

    Console.WriteLine("🔍 Debug - Emprunts à créer:");
    // Validate foreign keys before inserting
    foreach (var emp in emprunts)
    {
        Console.WriteLine($"  📋 Emprunt: Membre={emp.id_membre}, Inventaire={emp.Id_inv}, Biblio={emp.id_biblio}");
        
        if (!existingMembres.Contains(emp.id_membre))
        {
            throw new InvalidOperationException($"Membre {emp.id_membre} not found in database. Available: {string.Join(", ", existingMembres)}");
        }
        if (!existingInventaires.Contains(emp.Id_inv))
        {
            throw new InvalidOperationException($"Inventaire {emp.Id_inv} not found in database. Available: {string.Join(", ", existingInventaires)}");
        }
        if (!existingBiblios.Contains(emp.id_biblio))
        {
            throw new InvalidOperationException($"Bibliothecaire {emp.id_biblio} not found in database. Available: {string.Join(", ", existingBiblios)}");
        }
    }

    await dbContext.Emprunts.AddRangeAsync(emprunts);
    await dbContext.SaveChangesAsync();
    Console.WriteLine($"✅ Seeded {emprunts.Count} emprunts");
    return emprunts;
}

}