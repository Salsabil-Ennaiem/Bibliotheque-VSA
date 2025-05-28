using domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

    public class SanctionConfiguration : IEntityTypeConfiguration<Sanction>
    {
        public void Configure(EntityTypeBuilder<Sanction> entity)
        {
            entity.ToTable("Sanction");
                entity.HasKey(e => e.id_sanc);

                entity.Property(e => e.id_sanc)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.id_membre)
                    .IsRequired();

                entity.Property(e => e.raison)
                    .HasConversion<string>()
                    .IsRequired();

                entity.Property(e => e.date_sanction)
                   .HasColumnType("timestamp with time zone")
             .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .IsRequired();

        entity.Property(e => e.date_fin_sanction)
            .HasColumnType("timestamp with time zone");

                entity.Property(e => e.montant)
                    .HasColumnType("decimal(100,3)");

                entity.Property(e => e.payement)
                    .HasColumnType("boolean");

                entity.Property(e => e.active)
                    .HasColumnType("boolean")
                    .HasDefaultValue(true);

                entity.Property(e => e.description)
                    .HasColumnType("text");



                entity.HasOne(e => e.Membre)
                    .WithMany(m => m.Sanctions)
                    .HasForeignKey(e => e.id_membre);

                entity.HasOne(e => e.Emprunt)
                    .WithMany(e => e.Sanctions)
                    .HasForeignKey(e => e.id_emp)
                    .IsRequired(false);

                entity.HasOne(e => e.Bibliothecaire)
                    .WithMany(b => b.Sanctions)
                    .HasForeignKey(e => e.id_biblio)
                    .IsRequired(false);
        }
    }
