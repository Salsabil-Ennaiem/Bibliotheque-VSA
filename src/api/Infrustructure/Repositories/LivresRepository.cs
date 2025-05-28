using Data;
using domain.Entity;
using domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositries
{
    public class LivresRepository : ILivresRepository
    {
        private readonly BiblioDbContext _dbContext;

        public LivresRepository(BiblioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<(Livres, Inventaire)>> GetAllAsync()
        {
            try
            {
                var query = from l in _dbContext.Set<Livres>()
                            join i in _dbContext.Set<Inventaire>()
                            on l.id_livre equals i.id_liv
                            select new { Livre = l, Inventaire = i };
                var results = await query.ToListAsync();
                return results.Select(x => (x.Livre, x.Inventaire));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving all Livres: {ex.Message}", ex);
            }
        }
        public async Task<(Livres, Inventaire)> GetByIdAsync(string id)
        {
            try
            {
                var query = from l in _dbContext.Set<Livres>()
                            join i in _dbContext.Set<Inventaire>()
                            on l.id_livre equals i.id_liv
                            where l.id_livre == id
                            select new { Livre = l, Inventaire = i };
                var result = await query.FirstOrDefaultAsync();
                if (result == null)
                {
                    throw new Exception($"Livre with ID {id} not found");
                }
                return (result.Livre, result.Inventaire);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving Livre with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<(Livres, Inventaire)> CreateAsync(Livres livre, Inventaire inventaire)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _dbContext.Livres.Add(livre);
                await _dbContext.SaveChangesAsync();
                inventaire.id_liv = livre.id_livre;
                _dbContext.Inventaires.Add(inventaire);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return (livre, inventaire);
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
                var existingPair = await GetByIdAsync(id);

                _dbContext.Entry(existingPair.Item1).CurrentValues.SetValues(livre);
                _dbContext.Entry(existingPair.Item2).CurrentValues.SetValues(inventaire);
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

                _dbContext.Set<Livres>().Remove(existingPair.Item1);
                _dbContext.Set<Inventaire>().Remove(existingPair.Item2);
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