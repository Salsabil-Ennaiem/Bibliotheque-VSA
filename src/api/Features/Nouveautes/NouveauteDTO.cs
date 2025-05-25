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
        public string? couverture { get; set; }
    }
    public class NauveauteGetALL
    {
        public DateTime date_publication { get; set; } = DateTime.UtcNow;
        public string? couverture { get; set; }
        public string? titre { get; set; }

    }
}