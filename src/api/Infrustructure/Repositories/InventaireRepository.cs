using Data;
using domain.Entity;
using domain.Interfaces;

namespace Infrastructure.Repositries
{
    public class InventaireRepository : Repository<Inventaire>, IInventaireRepository
    {
        public InventaireRepository(BiblioDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}