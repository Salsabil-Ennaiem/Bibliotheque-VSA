using domain.Entity;

namespace domain.Interfaces

{
    public interface IEmpruntsRepository : IRepository<Emprunts>
    {
        Task<Emprunts> GetByEmailAsync(string email);
    }
}
