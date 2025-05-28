namespace domain.Interfaces

{
    public interface IRepository<T> where T : class 
    {
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);    
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity, string id);
    Task DeleteAsync(string id);
    }
}
