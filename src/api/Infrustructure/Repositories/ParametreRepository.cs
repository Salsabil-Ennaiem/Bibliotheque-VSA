using Data;
using domain.Entity;
using domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositries
{
    public class ParametreRepository : IParametreRepository
    {
        private readonly Repository<Parametre> _repository;
        private readonly BiblioDbContext _context;

        public ParametreRepository(Repository<Parametre> repository, BiblioDbContext context)
        {
            _repository = repository;
            _context = context;
        }


        public async Task<Parametre> GetParam(string userId)
        {
            var parametre = await _context.Parametres
                .Where(p => p.IdBiblio == userId)
                .OrderByDescending(p => p.id_param)
                .FirstOrDefaultAsync();

            return parametre;
        }

        public async Task<Parametre> Updatepram(Parametre entity)
{  
    var existingParam = await GetParam(entity.IdBiblio);
    if (existingParam == null || !AreParametersEqual(existingParam, entity))
    {
        return await _repository.CreateAsync(entity);
    }
    return existingParam;
}

private bool AreParametersEqual(Parametre p1, Parametre p2)
{
    if (p1 == null || p2 == null) return false;

    return p1.Delais_Emprunt_Etudiant == p2.Delais_Emprunt_Etudiant &&
           p1.Delais_Emprunt_Enseignant == p2.Delais_Emprunt_Enseignant &&
           p1.Delais_Emprunt_Autre == p2.Delais_Emprunt_Autre &&
           p1.Modele_Email_Retard == p2.Modele_Email_Retard;
}


    }

}