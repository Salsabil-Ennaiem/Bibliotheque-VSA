import { Component, Input, OnInit } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { CarouselModule } from 'primeng/carousel';
import { TagModule } from 'primeng/tag';
import { SpeedDialModule } from 'primeng/speeddial';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Router } from '@angular/router';
import { NouveauteService } from '../../../../Services/nouveaute.service';
import { NouveauteGetALL } from '../../../../model/Nouveaute.model';
//import { NouveauteDTO, NouveauteGetALL } from '../../../../model/Nouveaute.model';
//import { NouveauteService } from '../../../../Services/Nouveaute.service';
 
@Component({
  selector: 'app-liste-nouveaute',
  imports: [ButtonModule, CarouselModule, TagModule, SpeedDialModule, CommonModule],
  templateUrl: './liste-nouveaute.component.html',
  styleUrl: './liste-nouveaute.component.css',
  providers: []
})
export class ListeNouveauteComponent implements OnInit {
 nouveautes: NouveauteGetALL[] = [];
 // isHosted = false; // selon votre logique

  @Input() isHosted: boolean = false;

  ngOnInit(): void {
    this.loadNouveautes();
  }
constructor(private router: Router , private nouveauteService: NouveauteService) {}
  loadNouveautes(): void {
    this.nouveauteService.getAll().subscribe({
      next: (data) => this.nouveautes = data,
      error: (err) => console.error('Erreur chargement nouveautés', err)
    });
  }

  showDetails(id: string | null): void {
    if (!id) return;
    this.router.navigate(['/nouveaute', id]);
  }

  onAjouter(): void {
    this.router.navigate(['/nouveaute/ajouter']);
  }

  getSpeedDialItems(id: string) {
    return [
      {
        label: 'Modifier',
        icon: 'pi pi-pencil',
        command: () => this.onModifier(id)
      },
      {
        label: 'Supprimer',
        icon: 'pi pi-trash',
        command: () => this.onSupprimer(id)
      }
    ];
  }

  onModifier(id: string): void {
    this.router.navigate(['/nouveaute/edit', id]);
  }

  onSupprimer(id: string): void {
    if (confirm('Voulez-vous vraiment supprimer cette nouveauté ?')) {
      this.nouveauteService.delete(id).subscribe({
        next: () => {
          // Rafraîchir la liste après suppression
          this.nouveautes = this.nouveautes.filter(n => n.id_nouv !== id);
        },
        error: (err) => {
          console.error('Erreur suppression nouveauté', err);
          alert('Erreur lors de la suppression.');
        }
      });
    }
  }
}
  