
export  enum TypeMemb
    {
        Etudiant="Etudiant",
        Enseignant="Enseignant",
        Autre="Autre"
    }
export  enum StatutMemb
    {
        actif="actif",
        sanctionne="sanctionne",
    }
export enum Statut_emp
    {
        en_cours ="en_cours",
        retourne ="retourne",
        perdu ="perdu"
    }
export interface EmppruntDTO {
    id_emp: string;
    date_emp: string;
    date_retour_prevu: string | null;
    date_effectif: string | null;
    statut_emp: Statut_emp;
    note: string | null;
    typeMembre: TypeMemb;
    nom: string | null;
    prenom: string | null;
    email: string | null;
    telephone: string | null;
    cin_ou_passeport: string ;
    statut: StatutMemb;
}

export interface UpdateEmppruntDTO {
    date_emp: string;
    date_retour_prevu: string | null;
    date_effectif: string | null;
    statut_emp: Statut_emp;
    note: string | null;
    typeMembre: TypeMemb;
    nom: string | null;
    prenom: string | null;
    email: string | null;
    telephone: string | null;
    cin_ou_passeport: string | null;
    statut: StatutMemb;
}

export interface CreateEmpRequest {
    date_emp: string;
    date_retour_prevu: string | null;
    note: string | null;
    typeMembre: TypeMemb;
    nom: string | null;
    prenom: string | null;
    email: string | null;
    telephone: string | null;
    cin_ou_passeport: string | null;
}