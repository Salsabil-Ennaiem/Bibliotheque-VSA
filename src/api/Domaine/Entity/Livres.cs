
namespace domain.Entity;

    public class Livres 
    {
        public string?  id_livre { get; set; }
        public string?  id_biblio { get; set; }
        public string? date_edition { get; set; } 
        public string? titre { get; set; }
        public string? auteur { get; set; }
        public string? isbn { get; set; }
        public string? editeur { get; set; }
        public string? couverture { get; set; }
        public virtual Bibliothecaire? Bibliothecaire { get; set; }
        public virtual ICollection<Inventaire>? Inventaires { get; set; }

    }








