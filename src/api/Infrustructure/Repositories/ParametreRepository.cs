using Data;
using domain.Entity;
using domain.Interfaces;

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

        public async Task<Parametre> GetParam()
        {
            var parametre = _context.Parametres.LastOrDefault();
            return await _repository.GetByIdAsync(parametre.id_param);
        }
        public async Task<Parametre> Updatepram(Parametre entity)
        {  
                var existingParam = await GetParam();
                if (entity != existingParam)
                {
                    return await _repository.CreateAsync(entity);
                }
                return existingParam;
        }
    }
   }