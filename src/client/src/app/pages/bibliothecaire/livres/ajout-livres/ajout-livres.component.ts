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

interface Livres {
  cote: string;
  editeur: string;
  date_edition: string;
  Etat_Livre: Etat_Liv | null;
  Titre: string;
  isbn: string;
  inventaire: string;
  auteur: string;
  image: string;
  description :string;
  langue: string;
}

interface Etat_Liv {
  label: string;
  value: string;
}

@Component({
  selector: 'app-ajout-livres',
  imports: [IconFieldModule, InputIconModule, InputTextModule, FormsModule, IftaLabelModule, CommonModule, ButtonModule, SelectModule , Textarea ],
  templateUrl: './ajout-livres.component.html',
  styleUrl: './ajout-livres.component.css'
})
export class AjoutLivresComponent {
selectEtat_Livre: Etat_Liv[] = [
    { label: 'Nuef', value: 'Nuef' },
    { label: 'Moyen', value: 'Moyen' },
    { label: 'Mauvais', value: 'Mauvais' }
  ];
  livre: Livres = {
    cote: '',
    editeur: '',
    Etat_Livre: this.selectEtat_Livre[2],
    date_edition: '',
    Titre: '',
    isbn: '',
    inventaire: '',
    image: '',
    auteur: '',
    description: '',
    langue: ''
  };
  Annuler() {
    console.log("annuler ok");
  }
  Ajouter() {
    console.log("ajouter ok");
  }
}
