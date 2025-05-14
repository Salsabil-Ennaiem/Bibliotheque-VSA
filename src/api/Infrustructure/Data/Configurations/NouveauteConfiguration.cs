using domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;
    public class NouveauteConfiguration : IEntityTypeConfiguration<Nouveaute>
    {
        public void Configure(EntityTypeBuilder<Nouveaute> entity)
        {
            entity.ToTable("Nouveaute");
                entity.HasKey(e => e.id_nouv);

                entity.Property(e => e.id_nouv)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.titre)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.fichier)
                    .HasColumnType("jsonb");

                entity.Property(e => e.description)
                    .HasColumnType("text");

                entity.Property(e => e.date_publication)
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP") ;

                entity.Property(e => e.couverture)
                    .HasMaxLength(200)
                    .HasDefaultValue("https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Favf.asso.fr%2Famboise%2Fwp-content%2Fuploads%2Fsites%2F171%2F2021%2F03%2FLogo-Nouveau.jpg&f=1&nofb=1&ipt=fdbaaa07e45eb9aa0e1f8802a963c3259485319662623816e07adf250d84f1f9")
                    .IsRequired(false);


                entity.HasOne(e => e.Bibliothecaire)
                    .WithMany(b => b.Nouveautes)
                    .HasForeignKey(e => e.id_biblio)
                    .OnDelete(DeleteBehavior.SetNull);
        }
    }
