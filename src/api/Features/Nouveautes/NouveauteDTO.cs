namespace api.Features.Nouveautes
{
    public class NouveauteDTO
    {
        public string? titre { get; set; }
        public Dictionary<string, object>? fichier { get; set; }
        public string? description { get; set; }
        public DateTime date_publication { get; set; } = DateTime.UtcNow;
        public string? couverture { get; set; } 

    }
    public class CreateNouveauteRequest
    {
        public string? titre { get; set; }
        public Dictionary<string, object>? fichier { get; set; }
        public string? description { get; set; }
        public string couverture { get; set; }="https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Favf.asso.fr%2Famboise%2Fwp-content%2Fuploads%2Fsites%2F171%2F2021%2F03%2FLogo-Nouveau.jpg&f=1&nofb=1&ipt=fdbaaa07e45eb9aa0e1f8802a963c3259485319662623816e07adf250d84f1f9";
    }
    public class NouveauteGetALL
    {
        public DateTime date_publication { get; set; } = DateTime.UtcNow;
        public string? couverture { get; set; }
        public string? titre { get; set; }

    }
}