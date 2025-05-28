using Data;
using domain.Entity;
using domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositries
{
    public class EmpruntsRepository : Repository<Emprunts>, IEmpruntsRepository
    {
        public EmpruntsRepository(BiblioDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
                public async Task<IEnumerable<Emprunts>> SearchAsync(string searchTerm)
        {
            var query = from e in _dbContext.Emprunts
                        where e.date_emp.ToString().Contains(searchTerm)
                           || (e.Id_inv != null && e.Id_inv.Contains(searchTerm))
                           || e.Statut_emp.ToString().Contains(searchTerm)
                           || (e.note != null && e.note.Contains(searchTerm))
                           || (e.id_membre != null && e.id_membre.Contains(searchTerm))
                        select new { Emprunts = e};
            

            var results = await query.ToListAsync();
            return results.Select(x => x.Emprunts);
        }
    }
}