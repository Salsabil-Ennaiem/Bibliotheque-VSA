
using domain.Entity.Enum;

namespace api.Features.Livre
{
    public class LivreDTO
    {
        public string?  id_livre { get; set; }
        public required string date_edition { get; set; }
        public required string titre { get; set; }
        public string? auteur { get; set; }
        public string? isbn { get; set; }
        public required string editeur { get; set; }
        public string? Description { get; set; }
        public string? Langue { get; set; }
        public string? couverture { get; set; }
        public string? cote_liv { get; set; }       
        public etat_liv? etat { get; set; }
        public Statut_liv statut { get; set; } 
        public string? inventaire {get; set;}
        
    }
public class UpdateLivreDTO
    {
        
        public required string date_edition { get; set; }
        public required string titre { get; set; }
        public string? auteur { get; set; }
        public string? isbn { get; set; }
        public required string editeur { get; set; }
        public string? Description { get; set; }
        public string? Langue { get; set; }
        public string? couverture { get; set; }
        public string? cote_liv { get; set; }       
        public etat_liv? etat { get; set; }
        public Statut_liv statut { get; set; } 
        public string? inventaire {get; set;}
        
    }

    public class CreateLivreRequest
    {
        public string? cote_liv { get; set; }
        public string? auteur { get; set; }
        public required string editeur { get; set; }
        public string? Langue { get; set; }
        public required string titre { get; set; }
        public string? isbn { get; set; }
        public string? inventaire { get; set; }
        public required string date_edition { get; set; }
        public etat_liv? etat { get; set; }
        public string? Description { get; set; }
        public string? couverture { get; set; }
    }

}