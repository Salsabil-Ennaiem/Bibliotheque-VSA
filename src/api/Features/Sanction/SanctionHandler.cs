using System.Security.Claims;
using domain.Interfaces;
using Mapster;

namespace api.Features.Sanction;

public class SanctionHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISanctionRepository _sanctionRepository;


    public SanctionHandler(IHttpContextAccessor httpContextAccessor, ISanctionRepository sanctionRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _sanctionRepository = sanctionRepository;
    }

    private string GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(userId == null)
        {
            throw new Exception("User not authenticated");
        }
        return userId;
    }

    public async Task<IEnumerable<SanctionDTO>> GetAllAsync()
    {
        var userId = GetUserId();
        var entities = await _sanctionRepository.GetAllAsync();
        var filtre = entities.Where(e => e.id_biblio == userId);
        return filtre.Adapt<IEnumerable<SanctionDTO>>();
    }

    public async Task<SanctionDTO> CreateAsync(CreateSanctionRequest createSanction)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var entity = createSanction.Adapt<domain.Entity.Sanction>();
        entity.id_biblio = userId;
        var created = await _sanctionRepository.CreateAsync(entity);
        return created.Adapt<SanctionDTO>();
    }

    public async Task<IEnumerable<domain.Entity.Sanction>> SearchAsync(string searchTerm)
    {
        var id = GetUserId();
        var entities = await _sanctionRepository.SearchAsync(searchTerm);
        var filtre = entities.Where(e => e.id_biblio == id);
        return filtre;
    }
    
}