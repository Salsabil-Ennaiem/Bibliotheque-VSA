using domain.Entity.Enum;

namespace domain.Entity;
    public class Emprunts 
    {
        public string? id_emp { get; set; }
        public string? id_membre { get; set; }
        public string? id_biblio { get; set; }
        public string? Id_inv { get; set; }
        public DateTime date_emp { get; set; }= DateTime.UtcNow;
        public DateTime? date_retour_prevu { get; set; }
        public DateTime? date_effectif { get; set; }
        public Statut_emp Statut_emp { get; set; }= Statut_emp.en_cours;
        public string? note { get; set; } 
        
        public virtual Membre? Membre { get; set; }
        public virtual Bibliothecaire? Bibliothecaire { get; set; }
        public virtual Inventaire? Inventaire { get; set; }
        public virtual ICollection<Sanction>? Sanctions { get; set; }
    }
