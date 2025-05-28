namespace api.Features.Profile;

public class ProfileDTO
{
   // public string? PhotoProfilUrl { get; set; }
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public string? Email { get; set; }
    public string? Telephone { get; set; }
}

public class UpdateProfileDto
{
    //public string? PhotoProfilUrl { get; set; }
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public string? Email { get; set; }
    public string? Telephone { get; set; }
    public string? AncienMotDePasse { get; set; }
    public string? NouveauMotDePasse { get; set; }
}
