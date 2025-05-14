using domain.Entity.Enum;

namespace domain.Entity;

    public class Sanction
    {
        public string?  id_sanc { get; set; }
        public string?  id_membre { get; set; }
        public string?  id_biblio { get; set; }
        public string?  id_emp { get; set; }
        public Raison_sanction raison { get; set; }
        public DateTime date_sanction { get; set; }=DateTime.UtcNow;
        public DateTime? date_fin_sanction { get; set; }
        public decimal? montant { get; set; } 
        public bool? payement { get; set; } 
        public bool active { get; set; } = true;
        public string? description { get; set; } 
        public virtual Membre? Membre { get; set; }
        public virtual Emprunts? Emprunt { get; set; }
        public virtual Bibliothecaire? Bibliothecaire { get; set; }
    }