using domain.Entity.Enum; 

namespace domain.Entity;

    public class Membre
    {
        public string?  id_membre { get; set; }
        public string?  id_biblio { get; set; }
        public TypeMemb TypeMembre { get; set; }
        public string? nom { get; set; }
        public string? prenom { get; set; }
        public string? email { get; set; }
        public string? telephone { get; set; }
        public string? cin_ou_passeport { get; set; }
        public DateTime date_inscription { get; set; } = DateTime.UtcNow;
        public StatutMemb Statut { get; set; } = StatutMemb.actif;
        public virtual Bibliothecaire? Bibliothecaire { get; set; }
        public virtual ICollection<Emprunts>? Emprunts { get; set; }
        public virtual ICollection<Sanction>? Sanctions { get; set; }
}
