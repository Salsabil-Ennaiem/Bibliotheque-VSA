using domain.Entity;

namespace domain.Interfaces
{
    public interface ISanctionRepository
    {
        Task<IEnumerable<Sanction>> GetAllAsync();
        Task<Sanction> CreateAsync(Sanction entity);
        Task<IEnumerable<Sanction>> SearchAsync(string searchTerm);  

    }
}
