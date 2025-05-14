using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using domain.Entity;


namespace Data.Configurations;
public class LivreConfiguration : IEntityTypeConfiguration<Livres>
{
    public void Configure(EntityTypeBuilder<Livres> entity)
    {
        entity.ToTable("Livres");
        entity.HasKey(e => e.id_livre);

        entity.Property(e => e.id_livre)
            .ValueGeneratedOnAdd();

        entity.Property(e => e.titre)
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(e => e.editeur)
            .HasMaxLength(100)
           .IsRequired();

        entity.Property(e => e.date_edition)
            .HasMaxLength(10)
           .IsRequired();           
        


        entity.Property(e => e.isbn)
            .HasMaxLength(13);

        entity.Property(e => e.couverture)
            .HasMaxLength(200);

        entity.Property(e => e.auteur)
            .HasMaxLength(100);



        entity.HasIndex(e => e.isbn)
            .IsUnique();


        entity.HasOne(e => e.Bibliothecaire)
            .WithMany(b => b.Livres)
            .HasForeignKey(e => e.id_biblio)
            .OnDelete(DeleteBehavior.SetNull);

        entity.HasMany(e => e.Inventaires)
            .WithOne(i => i.Livre)
            .HasForeignKey(i => i.id_inv)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
