import { CommonModule } from '@angular/common';
import { Component, ViewChild, ElementRef , EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DrawerModule } from 'primeng/drawer';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';

@Component({
  selector: 'app-profil',
  imports: [
    CommonModule,
    FormsModule,
    ButtonModule,
    DrawerModule,
    InputTextModule,
    TextareaModule
  ],
  templateUrl: './profil.component.html',
  styleUrls: ['./profil.component.css'],
})
export class ProfilComponent {
  @Input() visible = false;
  @Output() visibleChange = new EventEmitter<boolean>();

  nom = "Hedi";
  prenom = "Aicha";
  email = "email@example.com";
  telephone = "+216 34 567 289";
  description = "bibliothècaire full-stack avec 5 ans d'expérience";
  password = "admin";
  imageUrl = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse1.mm.bing.net%2Fth%3Fid%3DOIP.dDKYQqVBsG1tIt2uJzEJHwHaHa%26pid%3DApi&f=1&ipt=3ee187c8639dd106acc8f406950f7c47b184a1a01a46a471e70dcd53890bb3d8&ipo=images";

  isEditing = false;
  editableNom = this.nom;
  editablePrenom = this.prenom;
  editableEmail = this.email;
  editableTelephone = this.telephone;
  editableDescription = this.description;
  oldpassword = "";
  editablePassword = "";
  newImagePreview = this.imageUrl;


  onDrawerHide() {
    this.visible = false;
    this.cancelEditing();
    this.visibleChange.emit(false);
  }

  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  triggerFileInput() {
    this.fileInput.nativeElement.click();
  }

  handleFileUpload(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    if (!file.type.match(/image\/*/)) {
      console.log('Selected file:', file.name, file.type, file.size);
      alert('Veuillez sélectionner une image valide.');
      return;
    }

    const reader = new FileReader();
    reader.onload = (e: any) => {
      this.newImagePreview = e.target.result;
    };
    reader.readAsDataURL(file);
  }

  startEditing() {
    this.editableNom = this.nom;
    this.editablePrenom = this.prenom;
    this.editableEmail = this.email;
    this.editableTelephone = this.telephone;
    this.editableDescription = this.description;
    this.newImagePreview = this.imageUrl;
    this.isEditing = true;
  }

  cancelEditing() {
    this.isEditing = false;
  }

  saveChanges() {
    this.nom = this.editableNom;
    this.prenom = this.editablePrenom;
    this.email = this.editableEmail;
    this.telephone = this.editableTelephone;
    this.description = this.editableDescription;

    if (this.fileInput) {
      this.imageUrl = this.newImagePreview;
      
    }

    if (this.oldpassword == this.password && this.editablePassword !== "") {
      this.password = this.editablePassword;
      this.oldpassword= '';
      this.editablePassword = "";

    }
    
    this.newImagePreview = "";
    this.isEditing = false;

  }
}