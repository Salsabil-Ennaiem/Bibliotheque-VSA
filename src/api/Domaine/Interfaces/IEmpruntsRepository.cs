using domain.Entity;

namespace domain.Interfaces

{
    public interface IEmpruntsRepository : IRepository<Emprunts>
    {
        Task<IEnumerable<Emprunts>> SearchAsync(string searchTerm);  
    }
}
