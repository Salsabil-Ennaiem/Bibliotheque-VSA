import { Component } from '@angular/core';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';
import { IftaLabelModule } from 'primeng/iftalabel';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { Textarea } from 'primeng/textarea';

interface Emprunt {
  cote: string;
  editeur: string;
  typeEmprunteur: TypeEmprunteur | null;
  dateFinEmp:Date;
  email: string;
  nom: string;
  prenom: string;
  cin: string;
  telephone: string;
  notes: string;
}

interface TypeEmprunteur {
  label: string;
  value: string;
}
@Component({
  selector: 'app-ajout-sanction',
  imports: [IconFieldModule, InputIconModule, InputTextModule, FormsModule, IftaLabelModule, CommonModule, ButtonModule, SelectModule , Textarea],
  templateUrl: './ajout-sanction.component.html',
  styleUrl: './ajout-sanction.component.css'
})
export class AjoutSanctionComponent {
typesEmprunteur: TypeEmprunteur[] = [
    { label: 'Étudiant', value: 'etudiant' },
    { label: 'Enseignant', value: 'enseignant' },
    { label: 'Autre', value: 'autre' }
  ];
  emprunt: Emprunt = {
    cote: '',
    editeur: '',
    typeEmprunteur: this.typesEmprunteur[2],
    dateFinEmp: new Date(Date.now() + 10 * 24 * 60 * 60 * 1000),
    email: '',
    nom: '',
    prenom: '',
    cin: '',
    telephone: '',
    notes: ''
  };
  Annuler() {
    console.log("annuler ok");
  }
  Ajouter() {
    console.log("ajouter ok");
  }
}
