import { CommonModule } from '@angular/common';
import { Component, HostListener } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { ParametreComponent } from './parametre/parametre.component';
import { ProfilComponent } from './profil/profil.component';

@Component({
  selector: 'app-bibliothecaire',
  imports: [RouterOutlet, ButtonModule, CommonModule,RouterLink , ParametreComponent , ProfilComponent ] ,
  templateUrl: './bibliothecaire.component.html',
  styleUrl: './bibliothecaire.component.css'
})
export class BibliothecaireComponent {
  
  showParametre = false;
  showParametres() {
    this.showParametre= true;
  }

  showProfil = false;
  showProfile() {
    this.showProfil= true;
  }

  showNotifiaction = false;
  showNotification() {
    this.showNotifiaction= true;
  }

  isSticky: boolean = false;
  @HostListener('window:scroll', ['$event'])
  checkScroll() {
    this.isSticky = window.pageYOffset > 100;
  }
  scrollToTop(): void {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
}
