using Data;
using domain.Entity;
using domain.Interfaces;

namespace Infrastructure.Repositries
{
    public class LivresRepository : Repository<Livres>, ILivresRepository
    {
        public LivresRepository(BiblioDbContext dbContext)
            : base(dbContext)
        {}
        
    }
}
