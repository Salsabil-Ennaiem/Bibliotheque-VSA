import { Component, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { RouterLink } from '@angular/router';
import { ListeLivresComponent } from '../bibliothecaire/livres/liste-livres/liste-livres.component';
import { ListeNouveauteComponent } from '../bibliothecaire/nouveaute/liste-nouveaute/liste-nouveaute.component';
import { InputTextModule } from 'primeng/inputtext';


@Component({
  selector: 'app-accueil',
  imports: [CommonModule, FormsModule, ButtonModule, RippleModule, InputTextModule, RouterLink, ListeLivresComponent, ListeNouveauteComponent],
  templateUrl: './accueil.component.html',
  styleUrls: ['./accueil.component.css']
})
export class AccueilComponent {

  isSticky: boolean = false;
  isDarkMode: boolean = false;
  

  @HostListener('window:scroll', ['$event'])
  checkScroll() {
    this.isSticky = window.pageYOffset > 100;
  }

  scrollToTop(): void {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
  
  toggleTheme(): void {
    this.isDarkMode = !this.isDarkMode;
    if (this.isDarkMode) {
      document.body.classList.add('dark-theme');
    } else {
      document.body.classList.remove('dark-theme');
    }
  }

  

  
}
