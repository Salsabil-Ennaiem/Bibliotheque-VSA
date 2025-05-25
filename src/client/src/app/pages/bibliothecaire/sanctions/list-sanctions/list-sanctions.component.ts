import { Component, HostListener, OnInit } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { SelectModule } from 'primeng/select';
import { MenuItem } from 'primeng/api';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink, RouterOutlet } from '@angular/router';
import { InputTextModule } from 'primeng/inputtext';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';


interface Emprunt {
  id: string;
  id_Emprunteur: string;
  Cote_Liv: string;
  statut: string;
  DateRetourePrevu: Date;
  Date_Effectif?: Date;
  Date_Emprunt: Date;
  Note?: string;
  speedDialItems: MenuItem[];
  isFlipped: boolean;
  showSpeedDial: boolean;
}

interface FilterOption {
  label: string;
  value: string;
}

@Component({
  selector: 'app-list-sanctions',
  imports: [ InputIconModule , IconFieldModule, InputTextModule,RouterOutlet,
    ButtonModule, CardModule, SelectModule, CommonModule, FormsModule, RouterLink],
      templateUrl: './list-sanctions.component.html',
  styleUrl: './list-sanctions.component.css'
})
export class ListSanctionsComponent  implements OnInit {
  emprunts: Emprunt[] = [];
  filteredEmprunts: Emprunt[] = [];
  selectedFilter: string = 'all';
  filterOptions: FilterOption[] = [
    { label: 'Tous', value: 'all' },
    { label: 'En cours', value: 'en_cours' },
    { label: 'Retourné', value: 'retourne' },
    { label: 'Perdu', value: 'perdu' }
  ];

  ngOnInit() {
    this.emprunts = [
      {
        id: '1',
        id_Emprunteur: '123456789',
        DateRetourePrevu: new Date('2025-01-30'),
        statut: 'en_cours',
        Cote_Liv: 'A123456',
        Date_Emprunt: new Date('2025-01-15'),
        speedDialItems: this.createSpeedDialItems('1'),
        isFlipped: false,
        showSpeedDial: false
      },
      {
        id: '2',
        id_Emprunteur: '153456789',
        DateRetourePrevu: new Date('2025-01-10'),
        statut: 'en_cours',
        Cote_Liv: 'A123456',
        Date_Emprunt: new Date('2025-01-05'),
        speedDialItems: this.createSpeedDialItems('2'),
        isFlipped: false,
        showSpeedDial: false
      },
      {
        id: '3',
        id_Emprunteur: '123456789',
        DateRetourePrevu: new Date('2022-01-30'),
        statut: 'perdu',
        Cote_Liv: 'A123456',
        Date_Emprunt: new Date('2022-01-15'),
        Note: 'Prêt sans retoure',
        speedDialItems: this.createSpeedDialItems('3'),
        isFlipped: false,
        showSpeedDial: false
      },
      {
        id: '4',
        id_Emprunteur: '123456789',
        DateRetourePrevu: new Date('2025-01-30'),
        statut: 'retourne',
        Date_Effectif: new Date('2025-01-25'),
        Cote_Liv: 'A123456',
        Date_Emprunt: new Date('2025-01-15'),
        Note: 'Prêt personnel pour rénovation',
        speedDialItems: this.createSpeedDialItems('4'),
        isFlipped: false,
        showSpeedDial: false
      }
     
    ];
    this.filteredEmprunts = [...this.emprunts];
  }

  applyFilter() {
    if (this.selectedFilter === 'all') {
      this.filteredEmprunts = [...this.emprunts];
    } else {
      this.filteredEmprunts = this.emprunts.filter(emprunt => emprunt.statut === this.selectedFilter);
    }
  }

  searchResults: Emprunt[] = [];
  isInputVisible = false;
  searchQuery = '';
  toggleInput() {
    this.isInputVisible = true;
  }

  handleSearch(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.searchQuery = value;
    if (!this.searchQuery.trim()) {
      this.searchResults = [];
      return;
    }
    this.searchResults = this.emprunts.filter(e =>
      e.Cote_Liv.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
      e.Date_Emprunt.toString().includes(this.searchQuery.toLowerCase())
    );
  }
  @HostListener('document:click', ['$event'])
  @HostListener('window:scroll', [])
  @HostListener('document:keydown', ['$event'])
  handleOutsideEvents(event?: MouseEvent | KeyboardEvent) {
    if (event instanceof MouseEvent) {
      const clickedInside = this.isClickInside(event);
      if (!clickedInside) {
        this.isInputVisible = false;
      }
    } else if (event instanceof KeyboardEvent) {
      if (event.key === 'Escape') {
        this.isInputVisible = false;
      }
    } else {
      this.isInputVisible = false;
    }
  }

  isClickInside(event: MouseEvent): boolean {
    const searchContainer = document.getElementById('search-container');
    return searchContainer ? searchContainer.contains(event.target as Node) : false;
  }

  private createSpeedDialItems(empruntId: string): MenuItem[] {
    return [
      {
        label: 'Modifier',
        icon: 'pi pi-pencil',
        command: () => this.modifier(empruntId)
      },
      {
        label: 'Supprimer',
        icon: 'pi pi-trash',
        command: () => this.supprimer(empruntId)
      },
      {
        label: 'Sanction',
        icon: 'pi pi-ban',
        command: () => this.sanctionner(empruntId)
      }
    ];
  }

  toggleFlip(id: string) {
    this.emprunts.forEach(emprunt => {
      if (emprunt.id !== id) {
        emprunt.isFlipped = false;
      }
    });
    // Toggle the clicked card
    const emprunt = this.emprunts.find(e => e.id === id);
    if (emprunt) {
      emprunt.isFlipped = !emprunt.isFlipped;
    }
  }

  toggleSpeedDial(event: Event, id: string) {
    event.stopPropagation();
    const emprunt = this.emprunts.find(e => e.id === id);
    if (emprunt) {
      emprunt.showSpeedDial = !emprunt.showSpeedDial;
    }
  }

  importer() {
    console.log('Importer: Trigger file upload and send to backend');
  }

  exporter() {
    console.log('Exporter: Generate CSV and trigger download');
  }

  modifier(id: string) {
    console.log(`Modifier emprunt ID: ${id}`);
  }

  supprimer(id: string) {
    console.log(`Supprimer emprunt ID: ${id}`);
  }

  sanctionner(id: string) {
    console.log(`Afficher détails emprunt ID: ${id}`);
  }
}