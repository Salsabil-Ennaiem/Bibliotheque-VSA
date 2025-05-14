using domain.Entity.Enum;

namespace domain.Entity;

    public class Parametre
    {
    public string?  id_param { get; set; }
    public string?  IdBiblio { get; set; }
    public int Delais_Emprunt_Etudiant { get; set; }
    public int Delais_Emprunt_Enseignant { get; set; }
    public int Delais_Emprunt_Autre { get; set; }
    public string? Modele_Email_Retard { get; set; }
    public DateTime date_modification { get; set; } = DateTime.UtcNow;
    public virtual Bibliothecaire? Bibliothecaire { get; set; }
    public virtual Statistique? Statistiques { get; set; }
    }
