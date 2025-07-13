using domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

            public class ParametreConfiguration : IEntityTypeConfiguration<Parametre>
            {
                public void Configure(EntityTypeBuilder<Parametre> entity)
                {
                    entity.ToTable("Parametres");
                    entity.HasKey(e => e.id_param);
                    
                    entity.Property(e => e.id_param)
                    .ValueGeneratedOnAdd();

            entity.Property(e => e.date_modification)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
                    entity.Property(e => e.Delais_Emprunt_Autre)
                    .IsRequired();
                    
                    entity.Property(e => e.Delais_Emprunt_Enseignant)
                    .IsRequired();

                    entity.Property(e => e.Delais_Emprunt_Etudiant)
                    .IsRequired();

                    entity.Property(e => e.Modele_Email_Retard)
                    .IsRequired()
                    .HasMaxLength(1000);


        }
    }
