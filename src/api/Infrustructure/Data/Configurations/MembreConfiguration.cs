using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using domain.Entity;
using domain.Entity.Enum;

namespace Data.Configurations;

    public class MembreConfiguration : IEntityTypeConfiguration<Membre>
    {
        public void Configure(EntityTypeBuilder<Membre> entity)
        {
            entity.ToTable("Membre");
                entity.HasKey(e => e.id_membre);

                entity.Property(e => e.id_membre)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.cin_ou_passeport)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.nom)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.prenom)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.email)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.telephone)
                    .HasMaxLength(20);

                entity.Property(e => e.TypeMembre)
                    .HasConversion<string>()
                    .IsRequired();

                entity.Property(e => e.date_inscription)
                    .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP") ;

                entity.Property(e => e.Statut)
                    .HasConversion<string>()
                    .HasDefaultValue(StatutMemb.actif)
                    .IsRequired();


                entity.HasIndex(e => e.cin_ou_passeport)
                    .IsUnique();

                entity.HasIndex(e => e.email)
                    .IsUnique();


                entity.HasOne(e => e.Bibliothecaire)
                    .WithMany(b => b.Membres)
                    .HasForeignKey(e => e.id_biblio)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(e => e.Emprunts)
                    .WithOne(e => e.Membre)
                    .HasForeignKey(e => e.id_emp)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(e => e.Sanctions)
                    .WithOne(s => s.Membre)
                    .HasForeignKey(s => s.id_membre)
                    .OnDelete(DeleteBehavior.SetNull);
        }
    }
