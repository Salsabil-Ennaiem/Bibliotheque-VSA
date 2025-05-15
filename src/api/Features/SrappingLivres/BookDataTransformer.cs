using System.Text.RegularExpressions;
using domain.Entity;

namespace Features.ScrapingLivres;

public class BookDataTransformer
{
    private readonly ILogger<BookDataTransformer> _logger;

    public BookDataTransformer(ILogger<BookDataTransformer> logger)
    {
        _logger = logger;
    }

    public (Livres livre, Inventaire inventaire) Transform(RawBookData raw)
    {
        // Clean and normalize ISBN
        var isbn = CleanIsbn(raw.ISBN) ?? GenerateFallbackIsbn(raw.Title, raw.Author);
        
        // Extract year from various possible fields
        var year = ExtractYear(raw.Year) ?? DateTime.Now.Year.ToString();
        
        // Split title and author if they're combined
        var (title, author) = SplitTitleAndAuthor(raw.Title, raw.Author);
        
        var livre = new Livres
        {
            titre = CleanText(title),
            auteur = author != null ? CleanText(author) : null,
            isbn = isbn,
            editeur = raw.Publisher != null ? CleanText(raw.Publisher) : "Inconnu",
            date_edition = year,
            couverture = raw.CoverImage,
            id_biblio = "1" // Convert to string
        };
        
        var inventaire = new Inventaire
        {
            cote_liv = GenerateCote(isbn, raw.Publisher),
            etat = domain.Entity.Enum.etat_liv.moyen, // Use enum value
            statut = domain.Entity.Enum.Statut_liv.disponible, // Use enum value
            inventaire = $"Ajouté le {DateTime.Now:yyyy-MM-dd}"
        };
        
        _logger.LogInformation("Transformed book: {Title} (ISBN: {ISBN})", livre.titre, livre.isbn);
        return (livre, inventaire);
    }    
    private string? CleanIsbn(string? isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn)) return null;
        
        // Remove non-alphanumeric characters
        var cleaned = Regex.Replace(isbn, "[^0-9X]", "");
        return cleaned.Length >= 10 ? cleaned : null;
    }
    
    private string GenerateFallbackIsbn(string? title, string? author)
    {
        var baseString = $"{title}{author}{DateTime.Now.Ticks}";
        return Math.Abs(baseString.GetHashCode()).ToString("X");
    }
    
    private string? ExtractYear(string? yearText)
    {
        if (string.IsNullOrWhiteSpace(yearText)) return null;
        
        var match = Regex.Match(yearText, @"\d{4}");
        return match.Success ? match.Value : null;
    }
    
    private (string title, string? author) SplitTitleAndAuthor(string title, string? author)
    {
        if (!string.IsNullOrWhiteSpace(author)) return (title, author);
        
        // Try to extract author from title if in "Title - Author" format
        if (title.Contains(" - "))
        {
            var parts = title.Split(new[] { " - " }, 2, StringSplitOptions.RemoveEmptyEntries);
            return (parts[0].Trim(), parts.Length > 1 ? parts[1].Trim() : null);
        }
        
        return (title, null);
    }
    
    private string CleanText(string text)
    {
        return System.Net.WebUtility.HtmlDecode(text)
            .Replace("\n", " ")
            .Replace("\t", " ")
            .Trim();
    }
    
    private string GenerateCote(string? isbn, string? publisher)
    {
        var publisherCode = string.IsNullOrEmpty(publisher) 
            ? "UNK" 
            : publisher.ToUpper()[..Math.Min(3, publisher.Length)];
            
        return $"{publisherCode}-{isbn?.Substring(Math.Max(0, isbn.Length - 4)) ?? "XXXX"}";
    }
}