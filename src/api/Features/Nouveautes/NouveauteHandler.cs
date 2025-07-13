
using System.Security.Claims;
using domain.Entity;
using domain.Interfaces;
using Mapster;


namespace api.Features.Nouveautes
{
    public class NouveauteHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INouveauteRepository _nouveauteRepository;


        public NouveauteHandler( IHttpContextAccessor httpContextAccessor, INouveauteRepository nouveauteRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _nouveauteRepository = nouveauteRepository;
        }

        private string GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new Exception("User ID not found in the claims.");
            }
            return userId;
        }
//list genral
        public async Task<IEnumerable<NouveauteGetALL>> GetAllNouvAsync()
        {
            var rt = await _nouveauteRepository.GetAllNouvAsync();
            return rt.Adapt<IEnumerable<NouveauteGetALL>>();

        }
   //list specifique
        public async Task<IEnumerable<NouveauteGetALL>> GetAllAsync()
        {
            var userId = GetCurrentUserId();
            var entities = await _nouveauteRepository.GetAllAsync();
            var filtered = entities.Where(e => e.id_biblio == userId);
            return filtered.Adapt<IEnumerable<NouveauteGetALL>>();
        }
        public async Task<NouveauteDTO> GetByIdAsync(string id)
        {
            var userId = GetCurrentUserId();
            var entity = await _nouveauteRepository.GetByIdAsync(id);
            if (entity.id_biblio != userId)
            {
                throw new Exception("You are not authorized to access this resource.");
            }
            return entity.Adapt<NouveauteDTO>();
        }
        public async Task<NouveauteDTO> CreateAsync(CreateNouveauteRequest createNouveaute)
        {
              var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            /* var entity = new Nouveaute
          {
              id_nouv = Guid.NewGuid().ToString(),
              id_biblio = userId,
              titre = createNouveaute.titre,
              description = createNouveaute.description,
              couverture = createNouveaute.couverture,
              fichier = createNouveaute.fichier,
              date_publication = DateTime.UtcNow
          };*/
            var entity = createNouveaute.Adapt<Nouveaute>();
            entity.id_biblio=userId;
            var created = await _nouveauteRepository.CreateAsync(entity);
            return created.Adapt<NouveauteDTO>();
        }
        public async Task<NouveauteDTO> UpdateAsync(CreateNouveauteRequest nouveaute, string id)
        {
            var entity = nouveaute.Adapt<Nouveaute>();
            var Updated = await _nouveauteRepository.UpdateAsync(entity, id);
            return Updated.Adapt<NouveauteDTO>();
        }
        public async Task DeleteAsync(string id)
        {
            await _nouveauteRepository.DeleteAsync(id);
        }

    }   
}