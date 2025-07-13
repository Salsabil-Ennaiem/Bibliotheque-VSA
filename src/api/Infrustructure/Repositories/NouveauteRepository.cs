using Data;
using domain.Entity;
using domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositries
{
    public class NouveauteRepository : Repository<Nouveaute>, INouveauteRepository
    {
        private readonly BiblioDbContext _context;
        public NouveauteRepository(BiblioDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Nouveaute>> GetAllNouvAsync()
        {
            return await _context.Nouveautes.ToListAsync();
        }

    }

}