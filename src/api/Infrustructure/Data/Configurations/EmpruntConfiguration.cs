using domain.Entity;
using domain.Entity.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Data.Configurations;
    public class EmpruntConfiguration : IEntityTypeConfiguration<Emprunts>
    {
        public void Configure(EntityTypeBuilder<Emprunts> entity)
        {
            entity.ToTable("Emprunts");
                entity.HasKey(e => e.id_emp);

                entity.Property(e => e.id_emp)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_emp");   

                entity.Property(e => e.id_membre)
                    .IsRequired();

                entity.Property(e => e.Id_inv)
                    .IsRequired();

                entity.Property(e => e.date_emp)
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValue(DateTime.UtcNow)
                    .IsRequired();

                entity.Property(e => e.date_retour_prevu)
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.date_effectif)
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.Statut_emp)
                    .HasDefaultValue(Statut_emp.en_cours)
                    .HasConversion<string>()
                    .IsRequired();

                entity.Property(e => e.note)
                    .HasColumnType("text");


                entity.HasOne(e => e.Membre)
                    .WithMany(m => m.Emprunts)
                    .HasForeignKey(e => e.id_membre)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Bibliothecaire)
                    .WithMany(b => b.Emprunts)
                    .HasForeignKey(e => e.id_biblio)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Inventaire)
                    .WithMany(i => i.Emprunts)
                    .HasForeignKey(e => e.Id_inv)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(e => e.Sanctions)
                    .WithOne(s => s.Emprunt)
                    .HasForeignKey(s => s.id_emp)
                    .OnDelete(DeleteBehavior.SetNull);
        }
    }
