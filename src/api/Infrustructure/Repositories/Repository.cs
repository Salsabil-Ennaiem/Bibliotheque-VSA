
using Data;
using domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositries

{
    public class Repository<T> : IRepository<T> where T : class
    {
        public BiblioDbContext _dbContext;

        public Repository(BiblioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
        try
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving all {typeof(T).Name}: {ex.Message}", ex);
        }
        }
        public async Task<T> GetByIdAsync(string id)
        {
            try 
            {
                var entity = await _dbContext.Set<T>().FindAsync(id);
                if (entity == null)
                {
                    throw new Exception($"{typeof(T).Name} with ID {id} not found");
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving {typeof(T).Name} with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<T> CreateAsync(T entity)
        {
            try{
                _dbContext.Set<T>().Add(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating {typeof(T).Name}: {ex.Message}", ex);
            }
            
        }

        public async Task<T> UpdateAsync(T entity, string id)
        {
            
            var existingEntity = await GetByIdAsync(id);
            if (existingEntity == null)
                throw new Exception($"{typeof(T).Name} with ID {id} not found");
            try{

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating {typeof(T).Name} with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                throw new Exception($"{typeof(T).Name} with ID {id} not found");
            try{
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            Console.WriteLine($"{typeof(T).Name} with ID {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting {typeof(T).Name} with ID {id}: {ex.Message}", ex);
            }
           
        }
    }
}