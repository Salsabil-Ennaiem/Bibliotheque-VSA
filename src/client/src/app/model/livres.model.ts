

export enum Statut_liv
    {
        disponible ='Disponible',
        emprunté='Emprunté',
        perdu='Perdu'
    }

    export enum EtatLiv {
  Neuf = 'neuf',
  Moyen = 'moyen',
  Mauvais = 'mauvais'
    }
    
export interface LivreDTO {
    id_livre: string;
  date_edition: string;
  titre: string;
  auteur?: string;
  isbn?: string;
  editeur: string;
  description?: string;
  langue?: string;
  couverture?: string;
  cote_liv?: string;
  etat?: EtatLiv;
  statut?: Statut_liv; 
  inventaire?: string;
}
export interface UpdateLivreDTO {
  date_edition: string;
  titre: string;
  auteur?: string;
  isbn?: string;
  editeur: string;
  description?: string;
  langue?: string;
  couverture?: string;
  cote_liv?: string;
  etat?: EtatLiv;
  statut?: Statut_liv; 
  inventaire?: string;
}


export interface CreateLivreRequest {
  cote_liv?: string;
  auteur?: string;
  editeur: string;
  langue?: string;
  titre: string;
  isbn?: string;
  inventaire?: string;
  date_edition: string;
  etat?: EtatLiv;
  description?: string;
  couverture?: string;
}


