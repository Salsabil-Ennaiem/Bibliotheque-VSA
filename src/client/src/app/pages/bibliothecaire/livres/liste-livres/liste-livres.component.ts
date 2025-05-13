import { Component, HostListener, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { SpeedDialModule } from 'primeng/speeddial';
//import { SelectModule } from 'primeng/select';
import { MenuItem } from 'primeng/api';
import { RouterLink } from '@angular/router';

interface Livre {
  couverture: string;
  id: string;
  title: string;
  author: string;
  editeur: string;
  date_edition: string;
  ISBN:string;
  cote_liv: string;
  invtentaire: string;
  etat_liv: string;
  statut_liv: string;
  isOpen: boolean;
}
interface SelectItem {
  label: string;
  value: string;
}

@Component({
  selector: 'app-liste-livres',
  standalone: true,
  imports: [
   // SelectModule,
   RouterLink,
    CommonModule,
    FormsModule,
    ButtonModule,
    InputTextModule,
    IconFieldModule,
    InputIconModule,
    SpeedDialModule
  ],
  templateUrl: './liste-livres.component.html',
  styleUrls: ['./liste-livres.component.css']
})
export class ListeLivresComponent {
  @Input() isHosted: boolean = false;
  searchResults: Livre[] = [];
  isInputVisible = false;
  searchQuery = '';
  /*etat_liv: string[] = ['Mauvais', 'Moyen', 'Bien'];
  statut_liv: string[] = ['Disponible', 'Emprunté', 'Perdu'];*/
    selectedStatut: string ='';
  selectedEtat: string ='';
  
  etat_liv: SelectItem[] = [
    { label: 'Tous', value: '' },
    { label: 'Mauvais', value: 'Mauvais' },
    { label: 'Moyen', value: 'Moyen' },
    { label: 'Bien', value: 'Bien' }
  ];
  
  statut_liv: SelectItem[] = [
    { label: 'Tous', value: '' },
    { label: 'Disponible', value: 'Disponible' },
    { label: 'Emprunté', value: 'Emprunté' },
    { label: 'Perdu', value: 'Perdu' }
  ];
  livres: Livre[] = [
    {
      id: '1',
      title: 'Le Petit Prince',
      author: 'Antoine de Saint-Exupéry',
editeur : 'Gallimard',
      ISBN: '9782070412654',
      cote_liv: '1234567890',
      invtentaire: '1234567890',
      couverture: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse1.mm.bing.net%2Fth%3Fid%3DOIP.m3dSB_jJIuTfk5obG-eQggHaL0%26pid%3DApi&f=1&ipt=57bcc4ba3b487c802b416eb20f5378c03594b2cd4137445c660fa6b5605553a4&ipo=images',
      date_edition: '1943-04-06',
      etat_liv: 'Mauvais',
      statut_liv: 'Disponible',
      isOpen: false

  
    },
    {
      id: '2',
      title: 'L\'Étranger',
      author: 'Albert Camus',
      couverture: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse1.mm.bing.net%2Fth%3Fid%3DOIP.0lhZw7B4IUdvekIJNPpMDwHaLF%26pid%3DApi&f=1&ipt=f4a2adc9e93a8f1767f1aca373bc8893746af64bdc1d3859a388676c123d6cc4&ipo=images',
      date_edition: '1942-05-19',
      editeur : 'Gallimard',
      ISBN: '9782070412654',
      cote_liv: '1234567890',
      invtentaire: '1234567890',
      etat_liv: 'Mauvais',
      statut_liv: 'Emprunté',
      isOpen: false
    },
    // ... autres livres (inchangés, mais corrigés pour statut_liv)
    {
      id: '5',
      title: 'Notre-Dame de Paris',
      author: 'Victor Hugo',
      couverture: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse3.mm.bing.net%2Fth%3Fid%3DOIP.7opRIrV_aVA7Ra68eixlUwHaJk%26pid%3DApi&f=1&ipt=61c9b988388699751fbcc047458263379028c13173a6c87920e36e84a382a09e&ipo=images',
      date_edition: '1831-03-16',
      editeur : 'Gallimard',
      ISBN: '9782070412654',
      cote_liv: '1234567890',
      invtentaire: '1234567890',
      etat_liv: 'Mauvais',
      statut_liv: 'Disponible', // Corrigé de 'Bien' à une valeur valide
      isOpen: false
    },
    {
      id: '4',
      title: 'Les Misérables',
      author: 'Victor Hugo',
      couverture: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse3.mm.bing.net%2Fth%3Fid%3DOIP.0-Katif23FwC_-irjm-ltAHaJl%26pid%3DApi&f=1&ipt=18944956417869d5022f088c0f50ca21812f92ea2960395f49f790f64dcff37d&ipo=images',
      date_edition: '1862-04-03',
      editeur : 'Gallimard',
      ISBN: '9782070412654',
      cote_liv: '1234567890',
      invtentaire: '1234567890',
      etat_liv: 'Mauvais',
      statut_liv : 'Perdu',
      isOpen: false
    },
    {
      id: '5',
      title: 'Notre-Dame de Paris',
      author: 'Victor Hugo',
      couverture: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse3.mm.bing.net%2Fth%3Fid%3DOIP.7opRIrV_aVA7Ra68eixlUwHaJk%26pid%3DApi&f=1&ipt=61c9b988388699751fbcc047458263379028c13173a6c87920e36e84a382a09e&ipo=images',
      date_edition: '1831-03-16',
      editeur : 'Gallimard',
      ISBN: '9782070412654',
      cote_liv: '1234567890',
      invtentaire: '1234567890',
      etat_liv: 'Bien',
      statut_liv : 'Disponible',
      isOpen: false
    },
    {
      id: '6',
      title: 'Le Comte de Monte-Cristo',
      author: 'Alexandre Dumas',
      editeur : 'Gallimard',
      ISBN: '9782070412654',
      cote_liv: '1234567890',
      invtentaire: '1234567890',
      couverture: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse1.mm.bing.net%2Fth%3Fid%3DOIP.Av4mEg9lZTWgzSaucjKRUwAAAA%26pid%3DApi&f=1&ipt=bf49ef99e73167ab8d5fd722a6a6a2012ef3d2dff4741e06b84b189fd7c18d0b&ipo=images',
      date_edition: '1844-08-28',
      etat_liv: 'Mauvais',
      statut_liv : 'Disponible',
      isOpen: false
    },
    {
      id: '7',
      title: 'Germinal',
      author: 'Émile Zola',
      editeur : 'Gallimard',
      ISBN: '9782070412654',
      cote_liv: '1234567890',
      invtentaire: '1234567890',
      couverture: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse2.mm.bing.net%2Fth%3Fid%3DOIP.6KrwpMKbDjt9kl7OwS2eSwHaL1%26pid%3DApi&f=1&ipt=e8d90bebdbdbf8cfbd842a786c8f4e8799a9d030a1c5e1354974c06b684f2136&ipo=images',
      date_edition: '1885-03-02',
      etat_liv: 'Bien',
      statut_liv : 'Emprunté',
      isOpen: false
    },
    {
      id: '8',
      title: 'Bel-Ami',
      author: 'Guy de Maupassant',
      editeur : 'Gallimard',
      ISBN: '9782070412654',
      cote_liv: '1234567890',
      invtentaire: '1234567890',
      couverture: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse1.mm.bing.net%2Fth%3Fid%3DOIP.0lhZw7B4IUdvekIJNPpMDwHaLF%26pid%3DApi&f=1&ipt=f4a2adc9e93a8f1767f1aca373bc8893746af64bdc1d3859a388676c123d6cc4&ipo=images',
      date_edition: '1885-05-30',
      etat_liv: 'Mauvais',
      statut_liv : 'Disponible',
      isOpen: false
    },
    {
      id: '9',
      title: 'Le Rouge et le Noir',
      author: 'Stendhal',
      editeur : 'Gallimard',
      ISBN: '9782070412654',
      cote_liv: '1234567890',
      invtentaire: '1234567890',
      couverture: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse1.mm.bing.net%2Fth%3Fid%3DOIP.IvepMc571hRho-KwgRwESwHaLE%26pid%3DApi&f=1&ipt=a0b331821ca03fdc4f6a95fa1775868ee157f3177f2f4fbd502cbfbc0bfa0708&ipo=images',
      date_edition: '1830-11-13',
      etat_liv: 'Mauvais',
      statut_liv : 'Disponible',
      isOpen: false
    },
    {
      id: '10',
      title: 'Candide',
      author: 'Voltaire',
      editeur : 'Gallimard',
      ISBN: '9782070412654',
      cote_liv: '1234567890',
      invtentaire: '1234567890',
      couverture: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse1.mm.bing.net%2Fth%3Fid%3DOIP.PN0_oUobylRBO_I6OJKtSAHaIv%26pid%3DApi&f=1&ipt=75265613dc270b5725cbf7b566c948554eaf42baf577b61f0fa2a82e8ce635d1&ipo=images',
      date_edition: '1759-01-15',
      etat_liv: 'Mauvais',
      statut_liv : 'Disponible',
      isOpen: false
    },
    {
      id: '11',
      title: 'Candide',
      author: 'Voltaire',
      editeur : 'Gallimard',
      ISBN: '9782070412654',
      cote_liv: '1234567890',
      invtentaire: '1234567890',
      couverture: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse3.mm.bing.net%2Fth%2Fid%2FOIP.jUFw8IfTKeNANu4iNRt1agHaKo%3Fpid%3DApi&f=1&ipt=0be0309ccb0e7a140e83ce8bc6f56cd03c5627948a5016e931e22a6c6efed310&ipo=images',
      date_edition: '1759-01-15',
      etat_liv: 'Mauvais',
      statut_liv : 'Disponible',
      isOpen: false
    }
  
  ];

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
    this.searchResults = this.livres.filter(livre =>
      livre.title.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
      livre.author.toLowerCase().includes(this.searchQuery.toLowerCase())
    );
  }

  toggleBook(livre: Livre, event: Event) {
    event.stopPropagation();
    if (!livre.isOpen) {
      this.livres.forEach(l => l.isOpen = false);
      this.searchResults.forEach(l => l.isOpen = false);
    }
    livre.isOpen = !livre.isOpen;
  }

  getSpeedDialItems(livreId: string): MenuItem[] {
    return [

      {
        label: 'Modifier',
        icon: 'pi pi-pencil',
        command: () => this.editLivre(livreId)
      },
      {
        label: 'Supprimer',
        icon: 'pi pi-trash',
        command: () => this.deleteLivre(livreId)
      }
    ];
  }


  editLivre(livreId: string) {
    console.log(`Edit livre ID: ${livreId}`);
  }

  deleteLivre(livreId: string) {
    console.log(`Delete livre ID: ${livreId}`);
  }

  @HostListener('document:click', ['$event'])
  handleOutsideClick(event: MouseEvent) {
    const clickedInside = this.isClickInside(event);
    if (!clickedInside) {
      this.isInputVisible = false;
    }
  }

  isClickInside(event: MouseEvent): boolean {
    const searchContainer = document.getElementById('search-container');
    return searchContainer ? searchContainer.contains(event.target as Node) : false;
  }
}