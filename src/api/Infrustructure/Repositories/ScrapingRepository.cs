
using Data;
using domain.Entity;
using domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositries{

public class ScrapingRepository : IScrapingRepository
{
    public Task<bool> LivreExistsAsync(string isbn) => BookExistsAsync(isbn);
    
    public Task AddLivreWithInventaireAsync(Livres livre, Inventaire inventaire) => AddBookWithInventoryAsync(livre, inventaire);
    private readonly BiblioDbContext _db;
    private readonly ILogger<ScrapingRepository> _logger;

    public ScrapingRepository(BiblioDbContext db, ILogger<ScrapingRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<bool> BookExistsAsync(string isbn)
    {
        return await _db.Livres.AnyAsync(l => l.isbn == isbn);
    }

    public async Task AddBookWithInventoryAsync(Livres livre, Inventaire inventaire)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync();
        
        try
        {
            // Add book
            await _db.Livres.AddAsync(livre);
            await _db.SaveChangesAsync();
            
            // Set inventory relationship
            inventaire.id_liv = livre.id_livre;
            await _db.Inventaires.AddAsync(inventaire);
            
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
            
            _logger.LogInformation("Added new book: {Title} (ID: {Id})", livre.titre, livre.id_livre);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to add book with ISBN: {ISBN}", livre.isbn);
            throw;
        }
    }

    public async Task<int> ImportBooksAsync(IEnumerable<(Livres, Inventaire)> books)
    {
        var count = 0;
        
        foreach (var (livre, inventaire) in books)
        {
            try
            {
                if (livre.isbn != null && !await BookExistsAsync(livre.isbn))
                {
                    await AddBookWithInventoryAsync(livre, inventaire);
                    count++;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing book: {Title}", livre.titre);
            }
        }
        
        return count;
    }
}
}