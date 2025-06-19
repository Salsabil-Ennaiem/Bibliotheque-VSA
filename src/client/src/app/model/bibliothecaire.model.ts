export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;

}

export interface ForgotPasswordRequest {
  email: string;
}

export interface ResetPasswordRequest {
  email: string;
  token: string;
  newPassword: string;
  confirmPassword: string;
}


export interface ProfileDTO {
    id_biblio : string;
    nom: string | null;
    prenom: string | null;
    email: string | null;
    telephone: string | null;
}

export interface UpdateProfileDto {
    nom: string | null;
    prenom: string | null;
    email: string | null;
    telephone: string | null;
    ancienMotDePasse: string | null;
    nouveauMotDePasse: string | null;
}

