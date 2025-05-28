import { Component, Input, OnInit } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { CarouselModule } from 'primeng/carousel';
import { TagModule } from 'primeng/tag';
import { SpeedDialModule } from 'primeng/speeddial';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Product } from '../../../../model/product';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-liste-nouveaute',
  imports: [ButtonModule, CarouselModule, TagModule, SpeedDialModule, CommonModule, RouterLink],
  templateUrl: './liste-nouveaute.component.html',
  styleUrl: './liste-nouveaute.component.css',
  providers: []
})
export class ListeNouveauteComponent implements OnInit {
  @Input() isHosted: boolean = false;
  publication: Product[] | undefined;
  responsiveOptions: any[];
  items: MenuItem[] | undefined;

  constructor() {
    this.responsiveOptions = [
      {
        breakpoint: '1400px',
        numVisible: 2,
        numScroll: 1
      },
      {
        breakpoint: '1199px',
        numVisible: 3,
        numScroll: 1
      },
      {
        breakpoint: '767px',
        numVisible: 2,
        numScroll: 1
      },
      {
        breakpoint: '575px',
        numVisible: 1,
        numScroll: 1
      }
    ];
  }

  ngOnInit() {
  }

  getSeverity(status: string | undefined): string {
    console.log('getSeverity called with status:', status);
    if (!status) {
      console.warn('Inventory status is undefined');
      return 'info';
    }
    switch (status.toUpperCase()) {
      case 'INSTOCK':
        return 'success';
      case 'LOWSTOCK':
        return 'warning';
      case 'OUTOFSTOCK':
        return 'danger';
      default:
        console.warn('Unknown inventory status:', status);
        return 'info';
    }
  }

  getSpeedDialItems(productId: string): MenuItem[] {
    return [
      {
        label: 'Modifier',
        icon: 'pi pi-pencil',
        command: () => this.editProduct(productId)
      },
      {
        label: 'Supprimer',
        icon: 'pi pi-trash',
        command: () => this.deleteProduct(productId)
      }
    ];
  }

  addToCart(productId: string) {
    console.log(`Add to cart product ID: ${productId}`);
  }

  editProduct(productId: string) {
    console.log(`Edit product ID: ${productId}`);
  }

  deleteProduct(productId: string) {
    console.log(`Delete product ID: ${productId}`);
  }
}