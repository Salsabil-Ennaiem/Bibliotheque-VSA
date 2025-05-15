using Data;
using domain.Entity;
using domain.Interfaces;

namespace Infrastructure.Repositries
{
    public class NouveauteRepository : Repository<Nouveaute>, INouveauteRepository 
    {
        public NouveauteRepository(BiblioDbContext context) : base(context)
        {
        }

    }
    
}