
using System.Security.Claims;
using domain.Entity;
using domain.Interfaces;


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

        public async Task<IEnumerable<NouveauteGetALL>> GetAllAsync()
        {
            var entities = await _nouveauteRepository.GetAllAsync();
            return entities.Select(entity => new NouveauteGetALL
            {
                titre = entity.titre,
                date_publication = entity.date_publication,
                couverture = entity.couverture,
            });
        }

        public async Task<NouveauteDTO> GetByIdAsync(string id)
        {
            var entity = await _nouveauteRepository.GetByIdAsync(id);
            return  new NouveauteDTO
            {
                titre = entity.titre,
                description = entity.description,
                date_publication = entity.date_publication,
                couverture = entity.couverture,
                fichier = entity.fichier
            };
        }

        public async Task<NouveauteDTO> CreateAsync(CreateNouveauteRequest createNouveaute)
        {
              var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
               var entity = new Nouveaute
            {
                id_nouv = Guid.NewGuid().ToString(),
                id_biblio = userId,
                titre = createNouveaute.titre,
                description = createNouveaute.description,
                couverture = createNouveaute.couverture,
                fichier = createNouveaute.fichier,
                date_publication = DateTime.UtcNow
            };
            var created = await _nouveauteRepository.CreateAsync(entity);
            return new NouveauteDTO
            {
                titre = created.titre,
                description = created.description,
                date_publication = created.date_publication,
                couverture = created.couverture,
                fichier = created.fichier
            };
        }

        public async Task<NouveauteDTO> UpdateAsync(CreateNouveauteRequest nouveaute, string id)
        {
            var entity = new Nouveaute
            {
                titre = nouveaute.titre,
                description = nouveaute.description,
                couverture = nouveaute.couverture,
                fichier = nouveaute.fichier,
            };
            var Updated = await _nouveauteRepository.UpdateAsync(entity, id);
            return new NouveauteDTO
            {
                titre = Updated.titre,
                description = Updated.description,
                date_publication = Updated.date_publication,
                couverture = Updated.couverture,
                fichier = Updated.fichier
            };
        }
        public async Task DeleteAsync(string id)
        {
            await _nouveauteRepository.DeleteAsync(id);
        }

    }   
}