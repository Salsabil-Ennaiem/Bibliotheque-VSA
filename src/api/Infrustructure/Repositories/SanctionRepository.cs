using Data;
using domain.Entity;
using domain.Interfaces;

namespace Infrastructure.Repositries
{
    public class SanctionRepository : Repository<Sanction>, ISanctionRepository
    {
        public SanctionRepository(BiblioDbContext context) : base(context)
        {
        }
    }
}