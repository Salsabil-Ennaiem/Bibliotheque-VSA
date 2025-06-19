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
import { RouterLink } from '@angular/router';
import { EtatLiv, UpdateLivreDTO } from '../../../../model/Livres.model';
import { LivreService } from '../../../../Services/livre.service';

@Component({
  selector: 'app-modifier-livres',
  imports: [ RouterLink,IconFieldModule, InputIconModule, InputTextModule, FormsModule, IftaLabelModule, CommonModule, ButtonModule, SelectModule , Textarea ],
  templateUrl: './modifier-livres.component.html',
  styleUrl: './modifier-livres.component.css'
})
export class ModifierLivresComponent {
      livre: UpdateLivreDTO |any ;
      livreId: string |any;
    selectEtat_Livre: EtatLiv[] = [];
    constructor(private livreService: LivreService) { }
 
    editLivre(livreId: string, LivreDTO: UpdateLivreDTO) {
    console.log(`Edit livre ID: ${livreId}`);
    this.livreService.update(livreId, LivreDTO).subscribe(
      () => console.log('Livre updated successfully'),
      error => console.error('Error updating livre:', error)
    );
  }
}
