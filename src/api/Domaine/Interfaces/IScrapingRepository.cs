using domain.Entity;

namespace domain.Interfaces ;
public interface IScrapingRepository{
    Task<bool> LivreExistsAsync(string isbn);
    Task AddLivreWithInventaireAsync(Livres livre, Inventaire inventaire);
    Task<int> ImportBooksAsync(IEnumerable<(Livres, Inventaire)> books);
    }