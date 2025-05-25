using Data;
using domain.Entity;
using domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositries
{
    public class LivresRepository : Repository<Livres>, ILivresRepository
    {
        private readonly BiblioDbContext _dbContext;

        public LivresRepository(BiblioDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<(Livres, Inventaire)>> SearchAsync(string searchTerm)
        {
            var query = from l in _dbContext.Livres
                        join i in _dbContext.Inventaires
                        on l.id_livre equals i.id_liv
                        where (l.titre != null && l.titre.Contains(searchTerm))
                           || (l.auteur != null && l.auteur.Contains(searchTerm))
                           || (l.isbn != null && l.isbn.Contains(searchTerm))
                           || (l.date_edition != null && l.date_edition.Contains(searchTerm))
                           || (l.Description != null && l.Description.Contains(searchTerm))
                        select new { Livre = l, Inventaire = i };

            var results = await query.ToListAsync();
            return results.Select(x => (x.Livre, x.Inventaire));
        }
        
        
    }
}