
using System.Security.Claims;
using AutoMapper;
using domain.Entity;
using domain.Interfaces;

namespace api.Features.Nouveautes
{
    public class NouveauteHundler
    {
        private readonly INouveauteRepository _nouveauteRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public NouveauteHundler(INouveauteRepository nouveauteRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _nouveauteRepository = nouveauteRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<NauveauteGetALL>> GetAllAsync()
        {
            var entities = await _nouveauteRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<NauveauteGetALL>>(entities);
        }

        public async Task<NouveauteDTO> GetByIdAsync(string id)
        {
            var entity = await _nouveauteRepository.GetByIdAsync(id);
            return _mapper.Map<NouveauteDTO>(entity);
        }

        public async Task<NouveauteDTO> CreateAsync(CreateNouveauteRequest createNouveaute)
        {
              var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
               var entity = _mapper.Map<Nouveaute>(createNouveaute);
                entity.id_biblio = userId;
            var created = await _nouveauteRepository.CreateAsync(entity);
            return _mapper.Map<NouveauteDTO>(created);
        }

        public async Task<NouveauteDTO> UpdateAsync(CreateNouveauteRequest nouveaute, string id)
        {
            var entity = _mapper.Map<Nouveaute>(nouveaute);
            var created = await _nouveauteRepository.UpdateAsync(entity, id);
            return _mapper.Map<NouveauteDTO>(created);
        }
        public async Task DeleteAsync(string id)
        {
            await _nouveauteRepository.DeleteAsync(id);
        }

    }   
}