using domain.Entity;

namespace domain.Interfaces

{
    public interface IEmpruntsRepository : IRepository<Emprunts>
    {
        Task<IEnumerable<Emprunts>> GetOverdueEmpruntsAsync(string userId, DateTime currentDate);
        Task<IEnumerable<Emprunts>> SearchAsync(string searchTerm);  
    }
}
