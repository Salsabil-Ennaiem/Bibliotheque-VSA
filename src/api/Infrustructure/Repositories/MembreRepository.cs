using Data;
using domain.Entity;
using domain.Interfaces;

namespace Infrastructure.Repositries
{
    public class MembreRepository : Repository<Membre> , IMembreRepository 
    {
        public MembreRepository(BiblioDbContext dbContext) : base(dbContext)
        {
        }
    }
}