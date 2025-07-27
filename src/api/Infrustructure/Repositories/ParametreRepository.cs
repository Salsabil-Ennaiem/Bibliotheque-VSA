using Data;
using domain.Entity;
using domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositries
{
    public class ParametreRepository : IParametreRepository
    {
        private readonly BiblioDbContext _context;

        public ParametreRepository(Repository<Parametre> repository, BiblioDbContext context)
        {
            _context = context;
        }
        public async Task<Parametre> GetParam(string userId)
        {
            try
            {
                var parametre = await _context.Parametres
                    .Where(p => p.IdBiblio == userId)
                    .OrderByDescending(p => p.id_param)
                    .FirstOrDefaultAsync();
                return parametre;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving Parametre for User ID {userId}: {ex.Message}", ex);
            }
        }
        public async Task<Parametre> Updatepram(Parametre entity)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingParam = await GetParam(entity.IdBiblio);

                _context.Entry(existingParam).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return entity;
            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error updating Parametre for User ID {entity.IdBiblio}: {ex.Message}", ex);
            }

        }

        /*   private bool AreParametersEqual(Parametre p1, Parametre p2)
           {
               if (p1 == null || p2 == null) return false;

               return p1.Delais_Emprunt_Etudiant == p2.Delais_Emprunt_Etudiant &&
                      p1.Delais_Emprunt_Enseignant == p2.Delais_Emprunt_Enseignant &&
                      p1.Delais_Emprunt_Autre == p2.Delais_Emprunt_Autre &&
                      p1.Modele_Email_Retard == p2.Modele_Email_Retard;
           }
   */

    }

}