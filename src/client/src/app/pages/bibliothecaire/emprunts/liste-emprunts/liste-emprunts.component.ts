import { Component, ElementRef, HostListener, OnInit, ViewChild, viewChild } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { SpeedDialModule } from 'primeng/speeddial';
import { SelectModule } from 'primeng/select';
import { MenuItem } from 'primeng/api';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { InputTextModule } from 'primeng/inputtext';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { EmpruntService } from '../../../../Services/emprunt.service';
import { EmppruntDTO } from '../../../../model/Emprunts.model';

interface FilterOption {
  label: string;
  value: string;
}

@Component({
  selector: 'app-liste-emprunts',
  imports: [InputIconModule, IconFieldModule, InputTextModule,
    ButtonModule, CardModule, SpeedDialModule, SelectModule, CommonModule, FormsModule, RouterLink],
  templateUrl: './liste-emprunts.component.html',
  styleUrls: ['./liste-emprunts.component.css']
})
export class ListeEmpruntsComponent implements OnInit {
  /*emprunts: Emprunt[] = [];
  filteredEmprunts: Emprunt[] = [];
  selectedFilter: string = 'all';
  filterOptions: FilterOption[] = [
    { label: 'Tous', value: 'all' },
    { label: 'En cours', value: 'en_cours' },
    { label: 'Retourné', value: 'retourne' },
    { label: 'Perdu', value: 'perdu' }
  ];
*/
  //speedDialItems: MenuItem[] | any;
  isFlipped: boolean = false;
  showSpeedDial: boolean = false;

  /*  ngOnInit() {
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
   } */

  emprunts: EmppruntDTO[] = [];
  searchResults: EmppruntDTO[] = [];
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
    /*this.searchResults = this.emprunts.filter(e =>
      e.cin_ou_passeport.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
      e.date_emp.toString().includes(this.searchQuery.toLowerCase())*/
      this.EmpService.search(this.searchQuery).subscribe(
      data => this.searchResults = data,
      error => console.error('Error searching livres:', error)
    
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

  createSpeedDialItems(empruntId: string): MenuItem[] {
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
      if (emprunt.id_emp !== id) {
        this.isFlipped = false;
      }
    });
    // Toggle the clicked card
    const emprunt = this.emprunts.find(e => e.id_emp === id);
    if (emprunt) {
      this.isFlipped = !this.isFlipped;
    }
  }

  toggleSpeedDial(event: Event, id: string) {
    event.stopPropagation();
    const emprunt = this.emprunts.find(e => e.id_emp === id);
    if (emprunt) {
      this.showSpeedDial = !this.showSpeedDial;
    }
  }

  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  importer() {
    this.fileInput.nativeElement.click();
  }

  handleFileUpload(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;


    if (!file.type.match(/application\/(vnd.ms-excel|vnd.openxmlformats-officedocument.spreadsheetml.sheet)/)) {
      console.log('Selected file:', file.name, file.type, file.size);

      alert('Veuillez sélectionner un fichier Excel (.xls ou .xlsx).');
      return;
    }

    this.EmpService.import(file).subscribe(
      response => console.log('Import successful:', response),
      error => console.error('Error importing file:', error)
    );

  }
  constructor(private EmpService: EmpruntService, private router: Router) { }
  exporter() {
    this.EmpService.export().subscribe(
      blob => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'ListeEmprunts.xlsx';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
      },
      error => console.error('Error exporting file:', error)
    );
  }

  modifier(id: string) {
    console.log(`Navigating to edit Emprunts ID: ${id}`);
    this.router.navigate([`/bibliothecaire/emprunts/modifier/${id}`]);
  }


  supprimer(id: string) {
    console.log(`Delete Emprunts ID: ${id}`);
    this.EmpService.delete(id).subscribe(
      () => console.log('Emprunts deleted successfully'),
      error => console.error('Error deleting Emprunts:', error)
    );
  }

  sanctionner(id: string) {
    console.log(`Navigating to edit Emprunts ID: ${id}`);
    this.router.navigate([`/bibliothecaire/sanctions/ajouter/${id}`]);
  }


  ngOnInit() {
    this.EmpService.getAll().subscribe(
      emprunts => {
        this.emprunts = emprunts;
        //   this.filteredEmprunts = [...this.emprunts];
      },
      error => console.error('Error fetching Emprunts:', error)
    );
  }
}