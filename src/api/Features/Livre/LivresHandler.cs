using domain.Entity;
using domain.Entity.Enum;
using domain.Interfaces;
using NPOI.XSSF.UserModel;
using AutoMapper;
using System.Security.Claims;
using MathNet.Numerics;

namespace api.Features.Livre;

public class LivresHandler
{
    private readonly ILivresRepository _livresRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public LivresHandler(ILivresRepository livresRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _livresRepository = livresRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<(Livres, Inventaire)>> SearchAsync(string searchTerm)
    {
        return await _livresRepository.SearchAsync(searchTerm);
    }
    public async Task<IEnumerable<LivreDTO>> GetAllAsync()
    {
        var entities = await _livresRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<LivreDTO>>(entities);
    }
    public async Task<LivreDTO> GetByIdAsync(string id)
    {
        var entity = await _livresRepository.GetByIdAsync(id);
        return _mapper.Map<LivreDTO>(entity);
    }
    public async Task<LivreDTO> CreateAsync(CreateLivreRequest livredto)
    {
         var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
               var entity = _mapper.Map<Livres>(livredto);
                entity.id_biblio = userId;
            var created = await _livresRepository.CreateAsync(entity);
            return _mapper.Map<LivreDTO>(created);
    }
    public async Task<LivreDTO> UpdateAsync(LivreDTO livre, string id)
    {
        var entity = _mapper.Map<Livres>(livre);
        var created = await _livresRepository.UpdateAsync(entity , id);
        return _mapper.Map<LivreDTO>(created);    }
    public async Task DeleteAsync(string id)
    {
        await _livresRepository.DeleteAsync(id);
    }
    public async Task ImportAsync(Stream excelStream)
    {
        var workbook = new XSSFWorkbook(excelStream);
        var sheet = workbook.GetSheetAt(0);

        for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
        {
            var row = sheet.GetRow(rowIndex);
            if (row == null) continue;

            var livre = new Livres
            {
                id_livre = Guid.NewGuid().ToString(),
                id_biblio = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                date_edition = row.GetCell(2)?.StringCellValue,
                titre = row.GetCell(3)?.StringCellValue,
                auteur = row.GetCell(4)?.StringCellValue,
                isbn = row.GetCell(5)?.StringCellValue,
                editeur = row.GetCell(6)?.StringCellValue,
                Description = row.GetCell(7)?.StringCellValue,
                Langue = row.GetCell(8)?.StringCellValue,
                couverture = row.GetCell(9)?.StringCellValue
            };

            var inventaire = new Inventaire
            {
                id_inv = Guid.NewGuid().ToString(),
                id_liv = livre.id_livre,
                cote_liv = row.GetCell(12)?.StringCellValue,
                etat = row.GetCell(13)?.StringCellValue != null ? Enum.Parse<etat_liv>(row.GetCell(13).StringCellValue) : null,
                statut = row.GetCell(14)?.StringCellValue != null ? Enum.Parse<Statut_liv>(row.GetCell(14).StringCellValue) : default,
                inventaire = row.GetCell(15)?.StringCellValue
            };

            await _livresRepository.CreateAsync(livre);
        }

    }    public async Task<MemoryStream> ExportAsync()    {
        var data = await _livresRepository.SearchAsync(""); // Get all data

        var workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet("LivresInventaire");

        var headerRow = sheet.CreateRow(0);
        string[] headers = {
            "IdLivre", "IdBiblio", "DateEdition", "Titre", "Auteur", "ISBN", "Editeur", "Description", "Langue", "Couverture",
            "IdInv", "IdLiv", "CoteLiv", "Etat", "Statut", "Inventaire"
        };

        for (int i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        int rowIndex = 1;
        foreach (var (livre, inventaire) in data)
        {
            var row = sheet.CreateRow(rowIndex++);
            row.CreateCell(0).SetCellValue(livre.id_livre);
            row.CreateCell(1).SetCellValue(livre.id_biblio);
            row.CreateCell(2).SetCellValue(livre.date_edition);
            row.CreateCell(3).SetCellValue(livre.titre ?? "");
            row.CreateCell(4).SetCellValue(livre.auteur ?? "");
            row.CreateCell(5).SetCellValue(livre.isbn ?? "");
            row.CreateCell(6).SetCellValue(livre.editeur ?? "");
            row.CreateCell(7).SetCellValue(livre.Description ?? "");
            row.CreateCell(8).SetCellValue(livre.Langue ?? "");
            row.CreateCell(9).SetCellValue(livre.couverture ?? "");

            row.CreateCell(10).SetCellValue(inventaire.id_inv);
            row.CreateCell(11).SetCellValue(inventaire.id_liv);
            row.CreateCell(12).SetCellValue(inventaire.cote_liv ?? "");
            row.CreateCell(13).SetCellValue(inventaire.etat.ToString());
            row.CreateCell(14).SetCellValue(inventaire.statut.ToString());
            row.CreateCell(15).SetCellValue(inventaire.inventaire ?? "");
        }

        var stream = new MemoryStream();
        workbook.Write(stream);
        stream.Position = 0;
        return stream;
    }
}
