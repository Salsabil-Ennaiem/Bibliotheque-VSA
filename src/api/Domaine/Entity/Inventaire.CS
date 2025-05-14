using domain.Entity.Enum;

namespace domain.Entity;
public class Inventaire
{
    public string?  id_inv { get; set; } 
    public string?  id_liv { get; set; }
    public string? cote_liv { get; set; }       
    public etat_liv? etat { get; set; }= etat_liv.moyen;
    public Statut_liv statut { get; set; } = Statut_liv.disponible;
    public string? inventaire {get; set;}
    public virtual Livres? Livre { get; set; }
    public virtual ICollection<Emprunts>? Emprunts { get; set; }
}
