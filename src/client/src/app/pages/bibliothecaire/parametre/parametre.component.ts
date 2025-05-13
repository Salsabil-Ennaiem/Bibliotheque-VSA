import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DrawerModule } from 'primeng/drawer';
import { SliderModule } from 'primeng/slider';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';
import { TextareaModule } from 'primeng/textarea';

@Component({
  selector: 'app-parametre',
  imports: [  TextareaModule , InputTextModule, SliderModule, TagModule ,DrawerModule , FormsModule ,CommonModule  , SliderModule, ButtonModule],
  templateUrl: './parametre.component.html',
  styleUrl: './parametre.component.css'
})
export class ParametreComponent {
  @Input() visible = false;
  @Output() visibleChange = new EventEmitter<boolean>();
  
  onDrawerHide() {
    this.visible = false;
    this.visibleChange.emit(false);
    this.isEditing = false;
  }
  isEditing = false;
  emailTemplate = `Objet : Rappel de retour de livre en retard

Bonjour [Nom du Membre],

Nous vous rappelons que vous avez dépassé la date limite de retour pour le livre suivant :

Titre du livre : [Titre du Livre]
Date de retour prévue : [Date de Retour]

Nous vous serions reconnaissants de bien vouloir rapporter ce livre dans les plus brefs délais afin d’éviter toute pénalité supplémentaire.


Merci de votre compréhension.

Cordialement`;
  editableTemplate = '';
  delai_emp_prof=3;
  editableDelai_emp_prof=3;
  delai_emp_etu= 10;
  editableDelai_emp_etu= 10;
   delai_emp_autre= 10;
  editableDelai_emp_autre= 10;



  onSlideEnd(event: any) {
    this.delai_emp_prof = this.editableDelai_emp_prof;
    this.delai_emp_etu = this.editableDelai_emp_etu;
    this.delai_emp_autre = this.editableDelai_emp_autre;
  }


  startEditing() {
    this.editableTemplate = this.emailTemplate;
    this.editableDelai_emp_prof = this.delai_emp_prof;
    this.editableDelai_emp_etu = this.delai_emp_etu;
    this.editableDelai_emp_autre = this.delai_emp_autre;
    this.isEditing = true;
  }

  cancelEditing() {
    this.isEditing = false;
  }

  saveChanges() {
    this.emailTemplate = this.editableTemplate;
    this.delai_emp_prof = this.editableDelai_emp_prof;
    this.delai_emp_etu = this.editableDelai_emp_etu;
    this.delai_emp_autre = this.editableDelai_emp_autre;
    this.isEditing = false;
  }
}
