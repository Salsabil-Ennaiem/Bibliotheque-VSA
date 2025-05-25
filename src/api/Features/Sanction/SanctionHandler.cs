using System.Security.Claims;
using AutoMapper;
using domain.Entity;
using domain.Interfaces;
using Infrastructure.Repositries;

namespace api.Features.Sanction;

public class SanctionHandler
{
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISanctionRepository _sanctionRepository;


    public SanctionHandler(IMapper mapper, IHttpContextAccessor httpContextAccessor, ISanctionRepository sanctionRepository)
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _sanctionRepository = sanctionRepository;
    }

    public async Task<IEnumerable<SanctionDTO>> GetAllAsync()
    {
        var entities = await _sanctionRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<SanctionDTO>>(entities);
    }

    public async Task<SanctionDTO> CreateAsync(CreateSanctionRequest createSanction)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var entity = _mapper.Map<domain.Entity.Sanction>(createSanction);
        entity.id_biblio = userId;
        var created = await _sanctionRepository.CreateAsync(entity);
        return _mapper.Map<SanctionDTO>(created);
    }

    public async Task<IEnumerable<domain.Entity.Sanction>> SearchAsync(string searchTerm)
    {
        return await _sanctionRepository.SearchAsync(searchTerm);
    }
    
}