using Microsoft.AspNetCore.Identity;
using domain.Entity;
using domain.Entity.Enum;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seeders;

public static class DataSeeder
{
    
    public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
    {
        try
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<Bibliothecaire>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roleExists = await roleManager.RoleExistsAsync("ADMIN") && await roleManager.RoleExistsAsync("Bibliothecaire");
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole("Bibliothecaire"));
                await roleManager.CreateAsync(new IdentityRole("ADMIN"));
                Console.WriteLine("Created 'Bibliothecaire , ADMIN' role");
            }

            var users = new List<(string email, string password, string nom, string prenom)>
            {
                ("admin@example.com", "Admin@123", "Admin", "User"),
                ("biblio@example.com", "Biblio@123", "Marie", "Dupont")
            };

            var createdUserIds = new List<string>();

            foreach (var (email, password, nom, prenom) in users)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    var newUser = new Bibliothecaire
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        nom = nom,
                        prenom = prenom
                    };

                    var result = await userManager.CreateAsync(newUser, password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newUser, "Bibliothecaire");
                        Console.WriteLine($"✅ Seeded user {email}");
                        createdUserIds.Add(newUser.Id);
                    }
                }
                else
                {
                    Console.WriteLine($"ℹ️ User {email} already exists.");
                    createdUserIds.Add(user.Id);
                }
            }

            if (createdUserIds.Count >= 2)
            {
                await SeedAllDataAsync(serviceProvider, createdUserIds[0], createdUserIds[1]);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error seeding users: {ex.Message}");
        }
    }

    public static async Task SeedAllDataAsync(IServiceProvider serviceProvider, string biblio1Id, string biblio2Id)
    {
        try
        {
            var dbContext = serviceProvider.GetRequiredService<BiblioDbContext>();

            var existingBooks = await dbContext.Livres.AnyAsync();
            if (existingBooks)
            {
                Console.WriteLine("ℹ️ Data already exists in database.");
                return;
            }

            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var livres = await SeedLivresAsync(dbContext, biblio1Id, biblio2Id);
                var inventaires = await SeedInventairesAsync(dbContext, livres);
                var membres = await SeedMembresAsync(dbContext, biblio1Id, biblio2Id);

                var (ancienParametre, nouveauParametre) = await SeedParametresAsync(dbContext, biblio1Id, biblio2Id);

                // IMPORTANT: Save changes before creating emprunts to ensure all foreign keys exist
                await dbContext.SaveChangesAsync();

               var emprunts = await SeedEmpruntsAsync(dbContext, membres, inventaires, biblio1Id, biblio2Id);
                await SeedSanctionsAsync(dbContext, emprunts, membres, biblio1Id);
                await SeedNouveautesAsync(dbContext, biblio1Id, biblio2Id);
                await SeedStatistiquesAsync(dbContext, ancienParametre, nouveauParametre, emprunts, membres);

                await transaction.CommitAsync();
                Console.WriteLine("✅ All data seeded successfully!");
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error seeding all data: {ex.Message}");
        }
    }

    private static async Task<List<Livres>> SeedLivresAsync(BiblioDbContext dbContext, string biblio1Id, string biblio2Id)
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

    private static async Task<List<Inventaire>> SeedInventairesAsync(BiblioDbContext dbContext, List<Livres> livres)
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

    private static async Task<List<Membre>> SeedMembresAsync(BiblioDbContext dbContext, string biblio1Id, string biblio2Id)
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

    private static async Task SeedNouveautesAsync(BiblioDbContext dbContext, string biblio1Id, string biblio2Id)
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

    private static async Task<(Parametre ancien, Parametre nouveau)> SeedParametresAsync(BiblioDbContext dbContext, string biblio1Id, string biblio2Id)
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

private static async Task<List<Emprunts>> SeedEmpruntsAsync(BiblioDbContext dbContext, List<Membre> membres, List<Inventaire> inventaires, string biblio1Id, string biblio2Id)
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


    private static async Task SeedSanctionsAsync(BiblioDbContext dbContext, List<Emprunts> emprunts, List<Membre> membres, string biblio1Id)
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

    private static async Task SeedStatistiquesAsync(BiblioDbContext dbContext, Parametre ancienParametre, Parametre nouveauParametre, List<Emprunts> emprunts, List<Membre> membres)
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
