

export interface NouveauteDTO {
    id_nouv: string | null;
    titre: string | null;
    fichier: { [key: string]: any; } ;
    description: string | null;
    date_publication: string;
    couverture: string | null;
}

export interface CreateNouveauteRequest {
    titre: string | null;
    fichier: { [key: string]: any; } | null;
    description: string | null;
    couverture: string;
}

export interface NouveauteGetALL {
    id_nouv: string;
    date_publication: string;
    couverture: string | null;
    titre: string | null;
}

