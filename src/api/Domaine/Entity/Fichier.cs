using domain.Entity;

namespace Domaine.Entity;

public class Fichier
{
    public required string IdFichier { get; set; }
    public string? NomFichier { get; set; }
    public string? CheminFichier { get; set; }
    public string? TypeFichier { get; set; }
    public required byte[] ContenuFichier { get; set; }
    public long TailleFichier { get; set; }
    public DateTime DateCreation { get; set; }
    public string? NouveauteId { get; set; }
    public virtual Nouveaute? ficherNouv { get; set; }
    public virtual Nouveaute? couvertureNouv { get; set; }
    public virtual Livres? Livre { get; set; }
    public virtual Bibliothecaire? Bibliothecaire { get; set; }
    
}
