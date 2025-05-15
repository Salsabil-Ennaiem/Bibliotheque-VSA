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
        }
        public async Task<Emprunts> GetByEmailAsync(string note)
        {
            try
            {
                var utilisateur = await _dbContext.Set<Emprunts>().AsNoTracking().FirstOrDefaultAsync(u => u.note == note);

                if (utilisateur == null)
                {
                    throw new System.Exception($"Utilisateur with email {note} not found.");
                }

                return utilisateur;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception($"Error fetching record with email {note}: " + ex.Message, ex);
            }
        }

    }
}
