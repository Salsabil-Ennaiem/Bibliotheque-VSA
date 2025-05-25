using System.Security.Claims;
using AutoMapper;
using domain.Entity;
using domain.Entity.Enum;
using domain.Interfaces;
using Infrastructure.Repositries;
using NPOI.XSSF.UserModel;

namespace api.Features.Emprunt;

public class EmpruntHandler
{
    private readonly IEmpruntsRepository _empruntsRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public EmpruntHandler(IEmpruntsRepository empruntsRepository , IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _empruntsRepository = empruntsRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<Emprunts>> SearchAsync(string searchTerm)
    {
        return await _empruntsRepository.SearchAsync(searchTerm);
    }
    public async Task<IEnumerable<EmppruntDTO>> GetAllAsync()
    {
        var entities = await _empruntsRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<EmppruntDTO>>(entities);
    }
    public async Task<EmppruntDTO> GetByIdAsync(string id)
    {
        var entity = await _empruntsRepository.GetByIdAsync(id);
        return _mapper.Map<EmppruntDTO>(entity);
    }
    public async Task<EmppruntDTO> CreateAsync(CreateEmpRequest empdto)
    {
         var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
               var entity = _mapper.Map<Emprunts>(empdto);
                entity.id_biblio = userId;
            var created = await _empruntsRepository.CreateAsync(entity);
            return _mapper.Map<EmppruntDTO>(created);
    }
    public async Task<EmppruntDTO> UpdateAsync(EmppruntDTO emp, string id)
    {
        var entity = _mapper.Map<Emprunts>(emp);
        var created = await _empruntsRepository.UpdateAsync(entity , id);
        return _mapper.Map<EmppruntDTO>(created);    }
    public async Task DeleteAsync(string id)
    {
        await _empruntsRepository.DeleteAsync(id);
    }
    public async Task ImportAsync(Stream excelStream)
    {
        var workbook = new XSSFWorkbook(excelStream);
        var sheet = workbook.GetSheetAt(0);

        for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
        {
            var row = sheet.GetRow(rowIndex);
            if (row == null) continue;

            var emp = new Emprunts
            {
                id_emp = Guid.NewGuid().ToString(),
                id_membre = row.GetCell(0)?.StringCellValue,
                id_biblio = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Id_inv = row.GetCell(1)?.StringCellValue,
                date_emp = DateTime.UtcNow,
                date_retour_prevu = row.GetCell(2)?.DateCellValue,
                date_effectif = row.GetCell(3)?.DateCellValue,
                Statut_emp = Enum.Parse<Statut_emp>(row.GetCell(4)?.StringCellValue ?? "en_cours"),
                note = row.GetCell(5)?.StringCellValue
            };

            await _empruntsRepository.CreateAsync(emp);
        }
    }

    public async Task<MemoryStream> ExportAsync()
    {
        var data = await _empruntsRepository.SearchAsync(""); // Get all data

        var workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet("Emprunts");

        var headerRow = sheet.CreateRow(0);
        string[] headers = {
            "IdEmprunt", "IdMembre", "IdBibliothecaire", "IdInventaire", "DateEmprunt", 
            "DateRetourPrevu", "DateEffectif", "StatutEmprunt", "Note"
        };

        for (int i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        int rowIndex = 1;
        foreach (var emprunt in data)
        {
            var row = sheet.CreateRow(rowIndex++);
            row.CreateCell(0).SetCellValue(emprunt.id_emp ?? "");
            row.CreateCell(1).SetCellValue(emprunt.id_membre ?? "");
            row.CreateCell(2).SetCellValue(emprunt.id_biblio ?? "");
            row.CreateCell(3).SetCellValue(emprunt.Id_inv ?? "");
            row.CreateCell(4).SetCellValue(emprunt.date_emp.ToString());
            row.CreateCell(5).SetCellValue(emprunt.date_retour_prevu?.ToString() ?? "");
            row.CreateCell(6).SetCellValue(emprunt.date_effectif?.ToString() ?? "");
            row.CreateCell(7).SetCellValue(emprunt.Statut_emp.ToString());
            row.CreateCell(8).SetCellValue(emprunt.note ?? "");
        }

        var stream = new MemoryStream();
        workbook.Write(stream);
        stream.Position = 0;
        return stream;
    }}