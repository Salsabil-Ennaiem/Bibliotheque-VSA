using System.Security.Claims;
using AutoMapper;
using domain.Entity;
using domain.Interfaces;
using Infrastructure.Repositries;

namespace api.Features.Parametre;

public class ParametreHandler
{
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IParametreRepository _parametreRepository;


    public ParametreHandler(IMapper mapper, IHttpContextAccessor httpContextAccessor, IParametreRepository parametreRepository)
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _parametreRepository = parametreRepository;
    }

        public async Task<ParametreDTO> GetByIdAsync()
        {
            var entity = await _parametreRepository.GetParam();
            return _mapper.Map<ParametreDTO>(entity);
        }

        public async Task<ParametreDTO> CreateAsync(ParametreDTO createNouveaute)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var entity = _mapper.Map<domain.Entity.Parametre>(createNouveaute);
            entity.IdBiblio = userId;
            var created = await _parametreRepository.Updatepram(entity);
            return _mapper.Map<ParametreDTO>(created);
        }
}