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
    private string GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User is not authenticated.");
        return userId;
    }

    //specifique
    public async Task<IEnumerable<LivreDTO>> GetAllLivresAsync()
    {
        var userId = GetCurrentUserId();
        var entity = await _livresRepository.GetAllLivresAsync();
        var filtered = entity.Where(e => e.Item2.id_biblio == userId);
        return entity.Select(e => e.Item1.Adapt<LivreDTO>());

    }
    public async Task<IEnumerable<LivreDTO>> SearchAsync(string searchTerm)
    {
        try
        {
            var list = await _livresRepository.GetAllLivresAsync();
            var query = list.Where(l =>
              (l.Item1.titre != null && l.Item1.titre.Contains(searchTerm)) ||
              (l.Item1.auteur != null && l.Item1.auteur.Contains(searchTerm)) ||
              (l.Item1.isbn != null && l.Item1.isbn.Contains(searchTerm)) ||
              (l.Item1.date_edition != null && l.Item1.date_edition.Contains(searchTerm)) ||
              (l.Item1.Description != null && l.Item1.Description.Contains(searchTerm)) ||
              (l.Item2.cote_liv != null && l.Item2.cote_liv.Contains(searchTerm))
            );
            return query.Select(x => x.Adapt<LivreDTO>());

        }
        catch (Exception ex)
        {
            throw new Exception($"Error searching Livres: {ex.Message}", ex);
        }
    }
    //genrale
    public async Task<IEnumerable<LivreDTO>> GetAllAsync()
    {
        var entities = await _livresRepository.GetAllLivresAsync();
        return entities.Adapt<IEnumerable<LivreDTO>>();

    }
    public async Task<LivreDTO> GetByIdAsync(string id)
    {
        var entity = await _livresRepository.GetByIdAsync(id);
        return entity.Adapt<LivreDTO>();
    }
    public async Task<LivreDTO> CreateAsync(CreateLivreRequest livredto)
    {
        var userId = GetCurrentUserId();
        var livre = livredto.Adapt<Livres>();
        var inventaire = livredto.Adapt<Inventaire>();
        inventaire.id_biblio = userId;
        var createdLivre = await _livresRepository.CreateAsync(livre, inventaire);
        return createdLivre.Adapt<LivreDTO>();
    }
    public async Task<LivreDTO> UpdateAsync(UpdateLivreDTO livre, string id)
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
                    id_biblio = GetCurrentUserId(),
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
        catch (Exception ex)
        {
            throw new Exception("Erreur lors de l'importation des données depuis le fichier Excel", ex);
        }
        finally
        {
            workbook.Close();
        }
    }
    public async Task<MemoryStream> ExportAsync()
    {
        var data = await SearchAsync("");

        var workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet("LivresInventaire");

        var headerRow = sheet.CreateRow(0);
        string[] headers = {
        "IdLivre", "DateEdition", "Titre", "Auteur", "ISBN", "Editeur", "Description", "Langue", "Couverture",
         "CoteLiv", "Etat", "Statut", "Inventaire"
    };

        for (int i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        int rowIndex = 1;
        foreach (var livreDto in data) // Changed from tuple deconstruction to single object
        {
            var row = sheet.CreateRow(rowIndex++);

            // Book information (assuming these properties exist in LivreDTO)
            row.CreateCell(0).SetCellValue(livreDto.id_livre ?? "");
            row.CreateCell(1).SetCellValue(livreDto.date_edition ?? "");
            row.CreateCell(2).SetCellValue(livreDto.titre ?? "");
            row.CreateCell(3).SetCellValue(livreDto.auteur ?? "");
            row.CreateCell(4).SetCellValue(livreDto.isbn ?? "");
            row.CreateCell(5).SetCellValue(livreDto.editeur ?? "");
            row.CreateCell(6).SetCellValue(livreDto.Description ?? "");
            row.CreateCell(7).SetCellValue(livreDto.Langue ?? "");
            row.CreateCell(8).SetCellValue(livreDto.couverture ?? "");

            // Inventory information (assuming these properties exist in LivreDTO)

            row.CreateCell(9).SetCellValue(livreDto.cote_liv ?? "");
            row.CreateCell(10).SetCellValue(livreDto.etat?.ToString() ?? "");
            row.CreateCell(11).SetCellValue(livreDto.statut.ToString() ?? "");
            row.CreateCell(12).SetCellValue(livreDto.inventaire ?? "");
        }

        var stream = new MemoryStream();
        workbook.Write(stream);
        stream.Position = 0;

        workbook.Close();

        return stream;
    }


}
