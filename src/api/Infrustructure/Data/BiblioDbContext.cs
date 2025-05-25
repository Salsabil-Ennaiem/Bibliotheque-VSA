using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Data.Configurations;
using domain.Entity;


namespace Data;

public class BiblioDbContext : IdentityDbContext<Bibliothecaire>

{
 
        public BiblioDbContext(DbContextOptions<BiblioDbContext> options ) : base(options)
        {
           
        }
        

        public DbSet<Emprunts> Emprunts { get; set; }
        public DbSet<Inventaire> Inventaires { get; set; }
        public DbSet<Livres> Livres { get; set; }
        public DbSet<Bibliothecaire> Bibliothecaires { get; set; }
        public DbSet<Statistique> Statistiques { get; set; }
        public DbSet<Parametre> Parametres { get; set; }
        public DbSet<Nouveaute> Nouveautes { get; set; }
        public DbSet<Sanction> Sanctions { get; set; }
        public DbSet<Membre> Membres { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Call the base method to ensure Identity are created correctly

            modelBuilder.ApplyConfiguration(new EmpruntConfiguration());
            modelBuilder.ApplyConfiguration(new LivreConfiguration());
            modelBuilder.ApplyConfiguration(new InventaireConfiguration());
            modelBuilder.ApplyConfiguration(new BibliothecaireConfiguration());
            modelBuilder.ApplyConfiguration(new StatistiqueConfiguration());
            modelBuilder.ApplyConfiguration(new ParametreConfiguration());
            modelBuilder.ApplyConfiguration(new NouveauteConfiguration());
            modelBuilder.ApplyConfiguration(new SanctionConfiguration());
            modelBuilder.ApplyConfiguration(new MembreConfiguration());
        }
       
}
