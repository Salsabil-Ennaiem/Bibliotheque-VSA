using domain.Entity;
using Data;
using Microsoft.EntityFrameworkCore;
using Infrustructure.Seeders;

namespace Infrastructure.Seeders;

public static class DataSeeder
{


    public static async Task SeedAllDataAsync(IServiceProvider serviceProvider, string biblio1Id, string biblio2Id)
    {
        try
        {
            var dbContext = serviceProvider.GetRequiredService<BiblioDbContext>();

            // Check each table before seeding
            var hasLivres = await dbContext.Livres.AnyAsync();
            var hasInventaires = await dbContext.Inventaires.AnyAsync();
            var hasMembres = await dbContext.Membres.AnyAsync();
            var hasParametres = await dbContext.Parametres.AnyAsync();
            var hasEmprunts = await dbContext.Emprunts.AnyAsync();
            var hasSanctions = await dbContext.Sanctions.AnyAsync();
            var hasNouveautes = await dbContext.Nouveautes.AnyAsync();
            var hasStatistiques = await dbContext.Statistiques.AnyAsync();

            if (hasLivres && hasInventaires && hasMembres && hasParametres &&
                hasEmprunts && hasSanctions && hasNouveautes && hasStatistiques)
            {
                Console.WriteLine("ℹ️ Data already exists in database.");
                return;
            }
            //using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                List<Livres> livres = new List<Livres>();
                List<Inventaire> inventaires = new List<Inventaire>();
                List<Membre> membres = new List<Membre>();
                Parametre ancienParametre = null;
                Parametre nouveauParametre = null;
                List<Emprunts> emprunts = new List<Emprunts>();

                if (!hasLivres)
                {
                    Console.WriteLine("🌱 Seeding Livres...");
                    livres = await LivreSeeder.SeedLivresAsync(dbContext);
                }
                else
                {
                    Console.WriteLine("📚 Loading existing Livres...");
                    livres = await dbContext.Livres.ToListAsync();
                }
                if (!hasInventaires && livres.Any())
                {
                    Console.WriteLine("🌱 Seeding Inventaires...");
                    inventaires = await InventaireSeeder.SeedInventairesAsync(dbContext, livres, biblio1Id, biblio2Id);
                }
                else if (hasInventaires)
                {
                    Console.WriteLine("📦 Loading existing Inventaires...");
                    inventaires = await dbContext.Inventaires.ToListAsync();
                }

                // Seed ou récupérer les membres
                if (!hasMembres)
                {
                    Console.WriteLine("🌱 Seeding Membres...");
                    membres = await MembreSeeder.SeedMembresAsync(dbContext, biblio1Id, biblio2Id);
                }
                else
                {
                    Console.WriteLine("👥 Loading existing Membres...");
                    membres = await dbContext.Membres.ToListAsync();
                }

                // Seed ou récupérer les paramètres
                if (!hasParametres)
                {
                    Console.WriteLine("🌱 Seeding Parametres...");
                    (ancienParametre, nouveauParametre) = await parametreSeeder.SeedParametresAsync(dbContext, biblio1Id, biblio2Id);
                }
                else
                {
                    Console.WriteLine("⚙️ Loading existing Parametres...");
                    var parametres = await dbContext.Parametres.OrderBy(p => p.date_modification).ToListAsync();
                    if (parametres.Count >= 2)
                    {
                        ancienParametre = parametres[0];
                        nouveauParametre = parametres[1];
                    }
                    else if (parametres.Count == 1)
                    {
                        ancienParametre = nouveauParametre = parametres[0];
                    }
                }

                // Seed emprunts
                if (!hasEmprunts && membres.Any() && inventaires.Any())
                {
                    Console.WriteLine("🌱 Seeding Emprunts...");
                    emprunts = await EmpruntsSeeder.SeedEmpruntsAsync(dbContext, membres, inventaires, biblio1Id, biblio2Id);
                }
                else if (hasEmprunts)
                {
                    Console.WriteLine("📋 Loading existing Emprunts...");
                    emprunts = await dbContext.Emprunts.ToListAsync();
                }

                // Seed sanctions
                if (!hasSanctions && emprunts.Any() && membres.Any())
                {
                    Console.WriteLine("🌱 Seeding Sanctions...");
                    await SanctionSeeder.SeedSanctionsAsync(dbContext, emprunts, membres, biblio1Id);
                }
                else if (hasSanctions)
                {
                    Console.WriteLine("⚠️ Sanctions already exist.");
                }

                // Seed nouveautés
                if (!hasNouveautes)
                {
                    Console.WriteLine("🌱 Seeding Nouveautes...");
                    await NouveauteSeeder.SeedNouveautesAsync(dbContext, biblio1Id, biblio2Id);
                }
                else
                {
                    Console.WriteLine("🆕 Nouveautes already exist.");
                }

                // Seed statistiques
                if (!hasStatistiques && ancienParametre != null && nouveauParametre != null)
                {
                    Console.WriteLine("🌱 Seeding Statistiques...");
                    await StatistiqueSeeder.SeedStatistiquesAsync(dbContext, ancienParametre, nouveauParametre, emprunts, membres);
                }
                else if (hasStatistiques)
                {
                    Console.WriteLine("📊 Statistiques already exist.");
                }

                await dbContext.SaveChangesAsync();
                Console.WriteLine("✅ All data seeded successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error during seeding transaction: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error seeding all data: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }

}

