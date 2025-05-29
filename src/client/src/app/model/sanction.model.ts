
export enum Raison_sanction
    {
       retard="retard",
       perte="perte",
       degat ="degat", 
       autre="autre"
    }


export interface SanctionDTO {
    email: string | null;
    date_emp: string;
    raison: Raison_sanction;
    date_sanction: string;
    date_fin_sanction: string | null;
    montant: number | null;
    payement: boolean | null;
    active: boolean;
    description: string | null;
}

export interface CreateSanctionRequest {
    email: string | null;
    date_emp: string;
    raison: Raison_sanction;
    date_fin_sanction: string | null;
    montant: number | null;
    description: string | null;
}