using System.Security.Claims;
using domain.Entity;
using domain.Entity.Enum;
using domain.Interfaces;
using Mapster;
using NPOI.XSSF.UserModel;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Identity;

namespace api.Features.Emprunt;

public class EmpruntHandler
{
    private readonly IEmpruntsRepository _empruntsRepository;
    private readonly IRepository<Membre> _membreRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IParametreRepository _parametreRepository;
    private readonly UserManager<Bibliothecaire> _userManager;
    private readonly IConfiguration _configuration;

    public EmpruntHandler(UserManager<Bibliothecaire> userManager, IEmpruntsRepository empruntsRepository, IHttpContextAccessor httpContextAccessor, IParametreRepository parametreRepository, IConfiguration configuration)
    {
        _empruntsRepository = empruntsRepository;
        _httpContextAccessor = httpContextAccessor;
        _parametreRepository = parametreRepository;
        _userManager = userManager;
        _configuration = configuration;
    }

    private string GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User is not authenticated.");
        return userId;
    }

    public async Task<IEnumerable<EmppruntDTO>> GetAllAsync()
    {
        var userId = GetCurrentUserId();
        var entities = await _empruntsRepository.GetAllAsync();
        var filtered = entities.Where(e => e.id_biblio == userId);
        return filtered.Adapt<IEnumerable<EmppruntDTO>>();
    }

    public async Task<EmppruntDTO> GetByIdAsync(string id)
    {
        var userId = GetCurrentUserId();
        var entity = await _empruntsRepository.GetByIdAsync(id);
        if (entity.id_biblio != userId)
            throw new UnauthorizedAccessException("Access denied.");
        return entity.Adapt<EmppruntDTO>();
    }

    public async Task<IEnumerable<EmppruntDTO>> NotifyOverdueEmpruntsAsync()
    {
        var userId = GetCurrentUserId();
        var today = DateTime.UtcNow;

        var parametre = await _parametreRepository.GetParam(userId);
        if (parametre == null)
            throw new Exception("Parametre not found for the user.");

        var overdueEmprunts = await _empruntsRepository.GetOverdueEmpruntsAsync(userId, today);

        foreach (var emprunt in overdueEmprunts)
        {
            var message = parametre.Modele_Email_Retard
                ?.Replace("{IdEmprunt}", emprunt.id_emp)
                ?.Replace("{DateRetourPrevu}", emprunt.date_retour_prevu?.ToString("d"))
                ?? $"Your loan with ID {emprunt.id_emp} is overdue since {emprunt.date_retour_prevu?.ToShortDateString()}.";

            // Send email directly
            await SendEmailAsync(userId, "Overdue Loan Notification", message);
        }
        return overdueEmprunts.Adapt<IEnumerable<EmppruntDTO>>();
    }

    private async Task SendEmailAsync(string userId, string subject, string body)
    {
        var userEmail = await GetUserEmailByIdAsync(userId);
        if (string.IsNullOrEmpty(userEmail))
            throw new Exception($"Email not found for user {userId}");

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_configuration["MailSettings:Username"], _configuration["MailSettings:From"]));
        emailMessage.To.Add(new MailboxAddress("", userEmail));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(_configuration["MailSettings:Host"], int.Parse(_configuration["MailSettings:Port"]), true);
        await client.AuthenticateAsync(_configuration["MailSettings:Username"], _configuration["MailSettings:Password"]);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }

    private async Task<string> GetUserEmailByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new Exception($"User with ID {userId} not found.");

        return user.Email ?? throw new Exception($"Email not set for user with ID {userId}.");
    }

    public async Task<IEnumerable<Emprunts>> SearchAsync(string searchTerm)
    {
        var entities = await _empruntsRepository.SearchAsync(searchTerm);
        var id = GetCurrentUserId();
        var filtered = entities.Where(e => e.id_biblio == id);
        return filtered;

    }

    public async Task<EmppruntDTO> CreateAsync(CreateEmpRequest empdto)
    {
        var userId = GetCurrentUserId();

        // 1. Recherche du membre existant via le repository générique
        var allMembres = await _membreRepository.GetAllAsync(); 
        var membreExistant = allMembres.FirstOrDefault(m =>
            (!string.IsNullOrEmpty(empdto.cin_ou_passeport) && m.cin_ou_passeport == empdto.cin_ou_passeport) ||
            (!string.IsNullOrEmpty(empdto.email) && m.email == empdto.email)
        );

        // 2. Création du membre si inexistant
        if (membreExistant == null)
        {
            var nouveauMembre = empdto.Adapt<Membre>(); 
            nouveauMembre.id_membre = Guid.NewGuid().ToString();
            nouveauMembre.id_biblio = userId;

            membreExistant = await _membreRepository.CreateAsync(nouveauMembre);
        }

        // 3. Récupération des paramètres pour le délai d'emprunt
        var parametre = await _parametreRepository.GetParam(userId);
        if (parametre == null)
            throw new Exception("Parametre not found for the user.");

        // 4. Création de l'entité Emprunt
        var empruntEntity = empdto.Adapt<Emprunts>();
        empruntEntity.id_biblio = userId;
        empruntEntity.id_membre = membreExistant.id_membre;
        empruntEntity.date_emp = DateTime.UtcNow;

        int delayDays = empdto.TypeMembre switch
        {
            TypeMemb.Etudiant => parametre.Delais_Emprunt_Etudiant,
            TypeMemb.Enseignant => parametre.Delais_Emprunt_Enseignant,
            _ => parametre.Delais_Emprunt_Autre
        };

        empruntEntity.date_retour_prevu = empruntEntity.date_emp.AddDays(delayDays);

        // 5. Enregistrement de l'emprunt
        var createdEmprunt = await _empruntsRepository.CreateAsync(empruntEntity);

        return createdEmprunt.Adapt<EmppruntDTO>();
    }

    public async Task<EmppruntDTO> UpdateAsync(UpdateEmppruntDTO emp, string id)
    {
        var entity = emp.Adapt<Emprunts>();
        var created = await _empruntsRepository.UpdateAsync(entity, id);
        return created.Adapt<EmppruntDTO>();
    }
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
    }
}

