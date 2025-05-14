
namespace domain.Entity;

    public class Statistique
    {
    public string?  id_stat { get; set; }
    public string?  id_param { get; set; }
    public int Nombre_Sanction_Emises { get; set; }
    public decimal Somme_Amende_Collectées { get; set; }
    public double Taux_Emprunt_En_Perte { get; set; }
    public double Emprunt_Par_Membre { get; set; }
    public double Taux_Emprunt_En_Retard { get; set; }
    public int Période_en_jour { get; set; }
    public DateTime date_stat { get; set; } = DateTime.UtcNow;
    public virtual Parametre? Parametre { get; set; }
    }
