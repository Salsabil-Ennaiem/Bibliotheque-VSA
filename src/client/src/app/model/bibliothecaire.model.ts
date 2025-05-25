// to see datat 
export class Bibliothecaire {
    emailId: string;
    password: string;

    //to store 
    constructor() {
        this.emailId = '';
        this.password = '';
    }
}

export interface IBibliothecaireModel {
    userId: number
    emailId: string
    password: string
    createdDate: string
    projectName: string
    fullName: string
    mobileNo: string
    extraId: any
  }


