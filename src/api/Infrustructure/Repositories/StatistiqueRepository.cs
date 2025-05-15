using Data;
using domain.Entity;
using domain.Interfaces;

namespace Infrastructure.Repositries
{
    public class StatistiqueRepository :  Repository<Statistique> , IStatistiqueRepository 
    {
        public StatistiqueRepository(BiblioDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}