using api.Features.Livre;
using domain.Entity;
using Microsoft.AspNetCore.Routing.Tree;

namespace domain.Interfaces
{
    public interface ILivresRepository
    {
        Task<IEnumerable<(Livres,Inventaire)>> GetAllAsync();
        Task<(Livres ,Inventaire)> GetByIdAsync(string id);
        Task<(Livres ,Inventaire)> CreateAsync(Livres livre, Inventaire inventaire);
        Task<(Livres ,Inventaire)> UpdateAsync(Livres livre, Inventaire inventaire ,string id);
        Task DeleteAsync(string id);
        Task<IEnumerable<(Livres ,Inventaire)>> SearchAsync(string searchTerm);  

    }
}