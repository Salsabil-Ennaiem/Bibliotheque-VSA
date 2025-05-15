using Data;
using domain.Entity;
using domain.Interfaces;

namespace Infrastructure.Repositries
{
   public class ParametreRepository : Repository<Parametre>, IParametreRepository
   {
       public ParametreRepository(BiblioDbContext context) : base(context)
       {
       }
   }
}