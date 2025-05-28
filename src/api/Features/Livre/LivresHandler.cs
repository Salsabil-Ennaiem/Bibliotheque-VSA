using domain.Entity;
using domain.Entity.Enum;
using NPOI.XSSF.UserModel;
using System.Security.Claims;
using domain.Interfaces;
using Mapster;

namespace api.Features.Livre;

public class LivresHandler
{
    private readonly ILivresRepository _livresRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public LivresHandler(ILivresRepository livresRepository, IHttpContextAccessor httpContextAccessor)
    {
        _livresRepository = livresRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<LivreDTO>> SearchAsync(string searchTerm)
    {

        var entities = await _livresRepository.SearchAsync(searchTerm);
        return entities.Select(e => e.Item1.Adapt<LivreDTO>());
    }
    public async Task<IEnumerable<LivreDTO>> GetAllAsync()
    {
        var entities = await _livresRepository.GetAllAsync();
        return entities.Adapt<IEnumerable<LivreDTO>>();

    }
    public async Task<LivreDTO> GetByIdAsync(string id)
    {
        var entity = await _livresRepository.GetByIdAsync(id);
        return entity.Adapt<LivreDTO>();
    }
    public async Task<LivreDTO> CreateAsync(CreateLivreRequest livredto)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var livre = livredto.Adapt<Livres>();
        var inventaire = livredto.Adapt<Inventaire>();
        livre.id_biblio = userId;
        var createdLivre = await _livresRepository.CreateAsync(livre, inventaire);
        return createdLivre.Adapt<LivreDTO>();
    }
    public async Task<LivreDTO> UpdateAsync(LivreDTO livre, string id)
    {
        var entity = livre.Adapt<(Livres, Inventaire)>();
        var update = await _livresRepository.UpdateAsync(entity.Item1, entity.Item2, id);
        return update.Adapt<LivreDTO>();
    }
    public async Task DeleteAsync(string id)
    {
        await _livresRepository.DeleteAsync(id);
    }
    public async Task ImportAsync(Stream excelStream)
    {
        var workbook = new XSSFWorkbook(excelStream);
        var sheet = workbook.GetSheetAt(0);


        try
        {
            for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                if (row == null) continue;

                var livre = new Livres
                {
                    id_livre = Guid.NewGuid().ToString(),
                    id_biblio = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
                    date_edition = row.GetCell(2)?.StringCellValue ?? string.Empty,
                    titre = row.GetCell(3)?.StringCellValue ?? string.Empty,
                    auteur = row.GetCell(4)?.StringCellValue ?? string.Empty,
                    isbn = row.GetCell(5)?.StringCellValue ?? string.Empty,
                    editeur = row.GetCell(6)?.StringCellValue ?? string.Empty,
                    Description = row.GetCell(7)?.StringCellValue ?? string.Empty,
                    Langue = row.GetCell(8)?.StringCellValue ?? string.Empty,
                    couverture = row.GetCell(9)?.StringCellValue ?? string.Empty
                };

                Enum.TryParse(row.GetCell(13)?.StringCellValue, out etat_liv etat);
                Enum.TryParse(row.GetCell(14)?.StringCellValue, out Statut_liv statut);

                var inventaire = new Inventaire
                {
                    id_inv = Guid.NewGuid().ToString(),
                    id_liv = livre.id_livre,
                    cote_liv = row.GetCell(12)?.StringCellValue ?? string.Empty,
                    etat = etat,
                    statut = statut,
                    inventaire = row.GetCell(15)?.StringCellValue ?? string.Empty
                };

                // Use repository method to add entities
                await _livresRepository.CreateAsync(livre, inventaire);
            }

        }
        catch(Exception ex)
        {
            throw new Exception("Erreur lors de l'importation des données depuis le fichier Excel",ex );
        }
        finally
        {
            workbook.Close();
        }
    }
    public async Task<MemoryStream> ExportAsync()
    {
        var data = await _livresRepository.SearchAsync("");

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

        workbook.Close(); 

        return stream;
    }

}
