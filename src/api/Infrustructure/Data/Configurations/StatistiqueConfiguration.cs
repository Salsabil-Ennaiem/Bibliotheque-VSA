using domain.Entity;
using Microsoft.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace Data.Configurations;
    public class StatistiqueConfiguration : IEntityTypeConfiguration<Statistique>
    {
        public void Configure(EntityTypeBuilder<Statistique> builder)
        {
            builder.ToTable("Statistiques");
            builder.HasKey(e => e.id_stat);

            builder.Property(e => e.id_stat)
                    .ValueGeneratedOnAdd();

            

            builder.Property(e => e.Taux_Emprunt_En_Perte)
            .IsRequired();

            builder.Property(e => e.Somme_Amende_Collectées)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

            builder.Property(e => e.Taux_Emprunt_En_Retard)
            .IsRequired();

            builder.Property(e => e.Nombre_Sanction_Emises)
            .IsRequired();

            builder.Property(e => e.Emprunt_Par_Membre)
            .IsRequired();

            builder.Property(e => e.Période_en_jour)
            .IsRequired();

            builder.Property(e => e.date_stat)
           .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

            
            builder.HasOne(e => e.Parametre)
                  .WithOne(p => p.Statistiques)
                  .HasForeignKey<Statistique>(e => e.id_param)
                  .OnDelete(DeleteBehavior.SetNull);
        }
    }       
