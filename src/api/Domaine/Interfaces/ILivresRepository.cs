using domain.Entity;

namespace domain.Interfaces
{
    public interface ILivresRepository : IRepository<Livres>
    {
        Task<IEnumerable<(Livres, Inventaire)>> SearchAsync(string searchTerm);  
    }
}