using System.Security.Claims;
using domain.Entity;
using domain.Entity.Enum;
using domain.Interfaces;
using Mapster;

namespace api.Features.Parametre;

public class ParametreHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IParametreRepository _parametreRepository;
    private readonly IEmpruntsRepository _empruntsRepository;
    private readonly ISanctionRepository _sanctionRepository;


    public ParametreHandler(IHttpContextAccessor httpContextAccessor, ISanctionRepository sanctionRepository, IParametreRepository parametreRepository, IEmpruntsRepository empruntsRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _parametreRepository = parametreRepository;
        _empruntsRepository = empruntsRepository;
        _sanctionRepository = sanctionRepository;
    }

    public async Task<ParametreDTO> GetByIdAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var entity = await _parametreRepository.GetParam(userId);
        return entity.Adapt<ParametreDTO>();
    }

    public async Task<ParametreDTO> CreateAsync(ParametreDTO createNouveaute)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Utilisateur non authentifié.");

        var parametreEntity = createNouveaute.Adapt<domain.Entity.Parametre>();
        parametreEntity.IdBiblio = userId;
        var createdParametre = await _parametreRepository.Updatepram(parametreEntity);

        //var statistiqueEntity = await CalculerStatistiquesAsync(createdParametre);
        //await _statistiqueRepository.CreateAsync(statistiqueEntity);

        return createdParametre.Adapt<ParametreDTO>();
    }
/*
private async Task<Statistique> CalculerStatistiquesAsync(domain.Entity.Parametre parametre)
{
    // Exemple : récupérer les emprunts et sanctions liés à ce paramètre / utilisateur
    var emprunts = await _empruntsRepository.GetAllAsync(parametre.IdBiblio);
    var sanctions = await _sanctionRepository.GetAllAsync(parametre.IdBiblio);

    // Calculs simplifiés (à adapter selon votre logique métier)
    int nombreSanctions = sanctions.Count();
    decimal sommeAmendes = sanctions.Sum(s => s.MontantAmende); // supposez que MontantAmende existe
    double tauxEmpruntEnPerte = emprunts.Count(e => e.Statut_emp == Statut_emp.perdu) / (double)emprunts.Count();
    double empruntParMembre = emprunts.Count() / (double)await _membreRepository.CountByUserIdAsync(parametre.IdBiblio);
    double tauxEmpruntEnRetard = emprunts.Count(e => e.date_retour_prevu < DateTime.UtcNow && e.Statut_emp != Statut_emp.retourne) / (double)emprunts.Count();

    return new Statistique
    {
        id_stat = Guid.NewGuid().ToString(),
        id_param = parametre.id_param,
        Nombre_Sanction_Emises = nombreSanctions,
        Somme_Amende_Collectées = sommeAmendes,
        Taux_Emprunt_En_Perte = tauxEmpruntEnPerte,
        Emprunt_Par_Membre = empruntParMembre,
        Taux_Emprunt_En_Retard = tauxEmpruntEnRetard,
        Période_en_jour = 30, 
        date_stat = DateTime.UtcNow
    };
}

*/
}