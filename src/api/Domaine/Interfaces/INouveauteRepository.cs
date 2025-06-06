using domain.Entity;

namespace domain.Interfaces
{
    public interface INouveauteRepository : IRepository<Nouveaute>
    {
        Task<IEnumerable<Nouveaute>> GetAllNouvAsync();
    }
}
