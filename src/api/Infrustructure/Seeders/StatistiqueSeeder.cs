using domain.Entity;
using domain.Entity.Enum;
using Data;

namespace Infrastructure.Seeders;

public class StatistiqueSeeder
{
    public static async Task SeedStatistiquesAsync(BiblioDbContext dbContext, Parametre ancienParametre, Parametre nouveauParametre, List<Emprunts> emprunts, List<Membre> membres)
    {
        // Calculer les statistiques réelles basées sur les emprunts

        // Période 1: Entre ancien paramètre et nouveau paramètre (30 jours à 10 jours)
        var dateDebutPeriode1 = ancienParametre.date_modification;
        var dateFinPeriode1 = nouveauParametre.date_modification;
        var joursP1 = (dateFinPeriode1 - dateDebutPeriode1).Days; // 20 jours

        // Emprunts dans cette période
        var empruntsP1 = emprunts.Where(e => e.date_emp >= dateDebutPeriode1 && e.date_emp <= dateFinPeriode1).ToList();

        // RETARD = EN_COURS + date_retour_prevu dépassée
        var empruntsEnRetardP1 = empruntsP1.Where(e =>
            e.Statut_emp == Statut_emp.en_cours &&
            e.date_retour_prevu.HasValue &&
            e.date_retour_prevu < DateTime.UtcNow).Count();

        // PERTE = PERDU ou EN_COURS depuis plus de 1 an
        var empruntsPerteP1 = emprunts.Where(e =>
            e.Statut_emp == Statut_emp.perdu ||
            (e.Statut_emp == Statut_emp.en_cours && (DateTime.UtcNow - e.date_emp).Days > 365)).Count();

        // Période 2: Depuis nouveau paramètre jusqu'à maintenant (10 jours à aujourd'hui)
        var dateDebutPeriode2 = nouveauParametre.date_modification;
        var dateFinPeriode2 = DateTime.UtcNow;
        var joursP2 = (dateFinPeriode2 - dateDebutPeriode2).Days; // 10 jours

        var empruntsP2 = emprunts.Where(e => e.date_emp >= dateDebutPeriode2).ToList();

        // RETARD = EN_COURS + date_retour_prevu dépassée
        var empruntsEnRetardP2 = empruntsP2.Where(e =>
            e.Statut_emp == Statut_emp.en_cours &&
            e.date_retour_prevu.HasValue &&
            e.date_retour_prevu < DateTime.UtcNow).Count();

        var statistiques = new List<Statistique>
        {
            // Statistique pour la période avec ancien paramètre
            new Statistique
            {
                id_stat = Guid.NewGuid().ToString(),
                id_param = ancienParametre.id_param,
                Nombre_Sanction_Emises = 2,
                Somme_Amende_Collectées = 8.00m,
                Taux_Emprunt_En_Perte = emprunts.Count > 0 ? (double)empruntsPerteP1 / emprunts.Count * 100 : 0,
                Emprunt_Par_Membre = (double)empruntsP1.Count / membres.Count,
                Taux_Emprunt_En_Retard = empruntsP1.Count > 0 ? (double)empruntsEnRetardP1 / empruntsP1.Count * 100 : 0,
                Période_en_jour = joursP1,
                date_stat = dateFinPeriode1
            },
            
            // Statistique pour la période avec nouveau paramètre
            new Statistique
            {
                id_stat = Guid.NewGuid().ToString(),
                id_param = nouveauParametre.id_param,
                Nombre_Sanction_Emises = 1,
                Somme_Amende_Collectées = 0.00m,
                Taux_Emprunt_En_Perte = emprunts.Count > 0 ? (double)empruntsPerteP1 / emprunts.Count * 100 : 0,
                Emprunt_Par_Membre = (double)empruntsP2.Count / membres.Count,
                Taux_Emprunt_En_Retard = empruntsP2.Count > 0 ? (double)empruntsEnRetardP2 / empruntsP2.Count * 100 : 0,
                Période_en_jour = joursP2,
                date_stat = dateFinPeriode2
            }
        };

        await dbContext.Statistiques.AddRangeAsync(statistiques);
        await dbContext.SaveChangesAsync();
        Console.WriteLine($"✅ Seeded {statistiques.Count} statistiques calculées");
        Console.WriteLine($"  📊 Période 1: {joursP1} jours - {empruntsP1.Count} emprunts - {empruntsEnRetardP1} en retard");
        Console.WriteLine($"  📊 Période 2: {joursP2} jours - {empruntsP2.Count} emprunts - {empruntsEnRetardP2} en retard");
    }

}