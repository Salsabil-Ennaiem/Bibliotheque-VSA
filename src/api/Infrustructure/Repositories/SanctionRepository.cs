using Data;
using domain.Entity;
using domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositries
{
    public class SanctionRepository : ISanctionRepository
    {
        private readonly Repository<Sanction> _rep;
        private readonly BiblioDbContext _dbContext;
        public SanctionRepository(Repository<Sanction> repository, BiblioDbContext dbContext)
        {
            _dbContext = dbContext;
            _rep = repository;
        }
        public async Task<IEnumerable<Sanction>> GetAllAsync()
        {
            return await _rep.GetAllAsync();
        }

        public async Task<Sanction> CreateAsync(Sanction entity)
        {
            return await _rep.CreateAsync(entity);
        }

        public async Task<IEnumerable<Sanction>> SearchAsync(string searchTerm)
        {
            var query = from s in _dbContext.Sanctions
                        join e in _dbContext.Emprunts
                        on s.id_emp equals e.id_emp
                        where (s.description != null && s.description.Contains(searchTerm))
                           || s.date_sanction.ToString().Contains(searchTerm)
                           || e.date_emp.ToString().Contains(searchTerm)
                           || (e.id_membre != null && e.id_membre.Contains(searchTerm))
                           || (e.note != null && e.note.Contains(searchTerm))
                           || (e.Id_inv != null && e.Id_inv.Contains(searchTerm))
                        select new { Emprunts = e, Sanction = s };
                     
            var results = await query.ToListAsync();
            return results.Select(x => x.Sanction);
        }

    }
}