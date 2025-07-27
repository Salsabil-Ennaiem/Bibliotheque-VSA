using Domaine.Entity;
using Microsoft.AspNetCore.Identity;


namespace domain.Entity;

public class Bibliothecaire : IdentityUser

{
    public string? nom { get; set; }
    public string? prenom { get; set; }
    public string? adresse { get; set; }
    public string? Description { get; set; }
    public string? Photo { get; set; }
    public virtual ICollection<Inventaire>? Inventaires { get; set; }
    public virtual ICollection<Membre>? Membres { get; set; }
    public virtual ICollection<Emprunts>? Emprunts { get; set; }
    public virtual ICollection<Sanction>? Sanctions { get; set; }
    public virtual ICollection<Nouveaute>? Nouveautes { get; set; }
    public virtual ICollection<Parametre>? Parametres { get; set; }
    public virtual Fichier? Fichier { get; set; }
    }
