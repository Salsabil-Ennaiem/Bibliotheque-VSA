namespace api.Features.Parametre;

public class ParametreDTO
{
    public string?  id_param { get; set; }
    public int Delais_Emprunt_Etudiant { get; set; }
    public int Delais_Emprunt_Enseignant { get; set; }
    public int Delais_Emprunt_Autre { get; set; }
    public string? Modele_Email_Retard { get; set; }
}