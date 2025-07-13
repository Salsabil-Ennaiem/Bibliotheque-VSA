using Domaine.Entity;

namespace domain.Entity;

public class Nouveaute
{
    public string? id_nouv { get; set; }
    public string? id_biblio { get; set; }
    public string? titre { get; set; }
    public string? fichier { get; set; }
    public string? description { get; set; }
    public DateTime date_publication { get; set; } = DateTime.UtcNow;
    public string? couverture { get; set; }
    public virtual Bibliothecaire? Bibliothecaire { get; set; }
    public virtual ICollection<Fichier>? Fichiers { get; set; }
    public virtual Fichier? Couvertures { get; set; }

}
