using domain.Entity.Enum;

namespace api.Features.Emprunt;

public class EmppruntDTO
{
    public string? id_emp { get; set; }
    public DateTime date_emp { get; set; }
    public DateTime? date_retour_prevu { get; set; }
    public DateTime? date_effectif { get; set; }
    public Statut_emp Statut_emp { get; set; }
    public string? note { get; set; }
    public TypeMemb TypeMembre { get; set; }
    public string? nom { get; set; }
    public string? prenom { get; set; }
    public string? email { get; set; }
    public string? telephone { get; set; }
    public string? cin_ou_passeport { get; set; }
    public StatutMemb Statut { get; set; }
}
public class UpdateEmppruntDTO
{
    public DateTime date_emp { get; set; }
    public DateTime? date_retour_prevu { get; set; }
    public DateTime? date_effectif { get; set; }
    public Statut_emp Statut_emp { get; set; }
    public string? note { get; set; }
    public TypeMemb TypeMembre { get; set; }
    public string? nom { get; set; }
    public string? prenom { get; set; }
    public string? email { get; set; }
    public string? telephone { get; set; }
    public string? cin_ou_passeport { get; set; }
    public StatutMemb Statut { get; set; }
}
public class CreateEmpRequest
{
    public DateTime date_emp { get; set; }
    public DateTime? date_retour_prevu { get; set; }
    public string? note { get; set; }
    public TypeMemb TypeMembre { get; set; }
    public string? nom { get; set; }
    public string? prenom { get; set; }
    public string? email { get; set; }
    public string? telephone { get; set; }
    public string? cin_ou_passeport { get; set; }
}