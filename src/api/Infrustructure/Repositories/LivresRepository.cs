using Data;
using domain.Entity;
using domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;

namespace Infrastructure.Repositories
{
    public class LivresRepository : ILivresRepository
    {
        private readonly BiblioDbContext _dbContext;

        public LivresRepository(BiblioDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<(Livres, Inventaire)>> GetAllLivresAsync()
        {
            var query = from l in _dbContext.Livres
                        join i in _dbContext.Inventaires
                            on l.id_livre equals i.id_liv
                        select new { Livre = l, Inventaire = i };

            var results = await query.ToListAsync();

            return results.Select(x => (x.Livre, x.Inventaire));
        }
        public async Task<(Livres, Inventaire)> GetByIdAsync(string id)
        {
            try
            {
                // var userId = GetCurrentUserId();

                var query = from l in _dbContext.Livres
                            join i in _dbContext.Inventaires
                                on l.id_livre equals i.id_liv
                            where l.id_livre == id
                            // && l.id_biblio == userId
                            select new { Livre = l, Inventaire = i };

                var result = await query.FirstOrDefaultAsync();

                if (result == null)
                    throw new Exception($"Livre with ID {id} not found or access denied.");

                return (result.Livre, result.Inventaire);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving Livre with ID {id}: {ex.Message}", ex);
            }
        }
        public async Task<(Livres, Inventaire , string)> CreateAsync(Livres livre, Inventaire inventaire)
        {

            if ( await _dbContext.Livres.AnyAsync(l => l.titre == livre.titre && l.date_edition == livre.date_edition))
            {
                return (livre , inventaire ,"Failed to add book  it already exist");
            }
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await _dbContext.Livres.AddAsync(livre);
                await _dbContext.SaveChangesAsync();

                inventaire.id_liv = livre.id_livre;
                await _dbContext.Inventaires.AddAsync(inventaire);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return (livre, inventaire,"bien crée ");
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<(Livres, Inventaire)> UpdateAsync(Livres livre, Inventaire inventaire, string id)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {

                var Livre = await GetByIdAsync(id);
                _dbContext.Entry(Livre.Item1).CurrentValues.SetValues(livre);
                _dbContext.Entry(Livre.Item2).CurrentValues.SetValues(inventaire);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return (livre, inventaire);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error updating Livre with ID {id}: {ex.Message}", ex);
            }
        }
        public async Task DeleteAsync(string id)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {

                var existingPair = await GetByIdAsync(id);
                _dbContext.Livres.Remove(existingPair.Item1);
                _dbContext.Inventaires.Remove(existingPair.Item2);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                Console.WriteLine($"Livre with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error deleting Livre with ID {id}: {ex.Message}", ex);
            }
        }

    }
}
