using Data;
using domain.Entity;
using domain.Interfaces;

namespace Infrastructure.Repositries
{
    public class BibliothecaireRepository : Repository<Bibliothecaire>, IBibliothecaireRepository
    {
        public BibliothecaireRepository(BiblioDbContext dbContext)
            : base(dbContext)
        {
        }
       
    }
}