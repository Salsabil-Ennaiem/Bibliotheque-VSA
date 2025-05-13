import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { SelectItem } from 'primeng/api';
import { FormsModule } from '@angular/forms';
import { CardModule } from 'primeng/card';
import { SelectModule } from 'primeng/select';
import { ChartModule } from 'primeng/chart';
import { Chart, registerables } from 'chart.js';
import annotationPlugin from 'chartjs-plugin-annotation';

@Component({
  selector: 'app-tableaux-de-bord',
  imports: [FormsModule, CardModule, SelectModule, ChartModule],
  templateUrl: './tableaux-de-bord.component.html',
  styleUrl: './tableaux-de-bord.component.css'
})
export class TableauxDeBordComponent implements OnInit {
  // Données des graphiques
  topBooksData: any;
  rotationData: any;
  delayRateData: any;
  frequentDelaysData: any;
  sanctionData: any;
  lossCostData: any;
  monthlyLoansData: any;
  policyImpactData: any;

  // KPI
  averageLoanDuration: number = 14;
  totalLoans: number = 1000;
  delayRate: number = 30;
  sanctionRate: number = 15;
  lossCost: number = 5000;

  // Options du filtre
  categories: SelectItem[] = [
    { label: 'Tous', value: '' },
    { label: 'Economie', value: 'economie' },
    { label: 'IT', value: 'Informatique' },
    { label: 'Sciences', value: 'sciences' }
  ];
  selectedCategory: string = '';

  // Options des graphiques
  barOptions: any;
  lineOptions: any;
  doughnutOptions: any;
  stackedBarOptions: any;
  lineWithThresholdOptions: any;
  doubleLineOptions: any;

  constructor(private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    Chart.register(...registerables, annotationPlugin); 
    this.initChartOptions();
    this.initChartData();
  }

  initChartOptions() {
    this.barOptions = {
      indexAxis: 'y',
      plugins: { legend: { display: false } },
      scales: {
        x: { beginAtZero: true, title: { display: true, text: 'Nombre' } },
        y: { beginAtZero: true, title: { display: true, text: 'Livres/Utilisateurs' } }
      }
    };

    this.lineOptions = {
      plugins: { legend: { display: true, position: 'top' } },
      scales: {
        x: { title: { display: true, text: 'Mois' } },
        y: { beginAtZero: true, title: { display: true, text: 'Valeur' } }
      }
    };

    this.doughnutOptions = {
      plugins: { legend: { position: 'top' } }
    };

    this.stackedBarOptions = {
      plugins: { legend: { display: true, position: 'top' } },
      scales: {
        x: { stacked: true, title: { display: true, text: 'Mois' } },
        y: { stacked: true, beginAtZero: true, title: { display: true, text: 'Taux (%)' } }
      }
    };

    this.lineWithThresholdOptions = {
      plugins: { legend: { display: true, position: 'top' } },
      scales: {
        x: { id: 'x', title: { display: true, text: 'Mois' } },
        y: { id: 'y', beginAtZero: true, title: { display: true, text: 'Coût (dt)' } }
      },
      annotation: {
        annotations: {
          threshold: {
            type: 'line',
            scaleID: 'y',
            yMin: 500,
            yMax: 500,
            borderColor: '#595959', 
            borderWidth: 2,
            label: { content: 'Seuil d\'alerte', enabled: true }
          }
        }
      }
    };

    this.doubleLineOptions = {
      plugins: { legend: { display: true, position: 'top' } },
      scales: {
        x: { title: { display: true, text: 'Mois' } },
        y: { beginAtZero: true, title: { display: true, text: 'Taux de retard (%)' } }
      }
    };
  }

  initChartData() {
    this.topBooksData = {
      labels: ['Livre A', 'Livre B', 'Livre C', 'Livre D', 'Livre E', 'Livre F', 'Livre G', 'Livre H', 'Livre I', 'Livre J'],
      datasets: [{
        label: 'Emprunts',
        data: [120, 100, 90, 85, 80, 70, 60, 50, 40, 30],
        backgroundColor: ['#10451d', '#155d27', '#1a7431', '#208b3a', '#25a244', '#2dc653', '#4ad66d', '#6ede8a', '#92e6a7', '#b7efc5'] // Vert dégradé for top books
      }]
    };

    this.rotationData = {
      labels: ['Jan', 'Fév', 'Mar', 'Avr', 'Mai', 'Juin'],
      datasets: [
        {
          label: 'Taux de rotation',
          data: [0.5, 0.6, 0.55, 0.7, 0.65, 0.8],
          borderColor: '#25a244', 
          fill: false
        },
        {
          label: 'Livres inutilisés',
          data: [0.1, 0.15, 0.12, 0.2, 0.18, 0.25],
          borderColor: '#595959', 
          fill: false
        }
      ]
    };

    this.delayRateData = {
      labels: ['Retards >3 jours', 'Retours à temps'],
      datasets: [{
        data: [this.delayRate, 100 - this.delayRate],
        backgroundColor: ['#8c1c13', '#10451D'] 
      }]
    };

    this.frequentDelaysData = {
      labels: ['Utilisateur 1', 'Utilisateur 2', 'Utilisateur 3', 'Utilisateur 4', 'Utilisateur 5'],
      datasets: [{
        label: 'Retards',
        data: [15, 12, 10, 8, 5],
        backgroundColor: ['#250902', '#38040e', '#640d14', '#800e13', '#ad2831'] 
      }]
    };

    this.sanctionData = {
      labels: ['Jan', 'Fév', 'Mar', 'Avr', 'Mai', 'Juin'],
      datasets: [
        {
          label: 'Pertes',
          data: [5, 10, 8, 12, 6, 15],
          backgroundColor: '#6f1d1b', 
        },
        {
          label: 'Retards',
          data: [20, 25, 15, 30, 10, 20],
          backgroundColor: '#99582a', 
        },
        {
          label: 'autre',
          data: [2, 9, 5, 3, 1, 2],
          backgroundColor: '#adc178', 
        }
      ]
    };

    this.lossCostData = {
      labels: ['Jan', 'Fév', 'Mar', 'Avr', 'Mai', 'Juin'],
      datasets: [
        {
          label: 'Coût des pertes',
          data: [300, 450, 400, 600, 350, 700],
          borderColor: '#9d0208', 
          fill: false
        },
        {
          label: 'Amendes',
          data: [100, 150, 120, 200, 130, 250],
          borderColor: '#adc178', 
          fill: false
        }
      ]
    };

    this.monthlyLoansData = {
      labels: ['Jan', 'Fév', 'Mar', 'Avr', 'Mai', 'Juin'],
      datasets: [{
        label: 'Emprunts',
        data: [200, 250, 300, 350, 400, 450],
        borderColor: '#25a244', 
        fill: false
      }]
    };

    this.policyImpactData = {
      labels: ['Jan', 'Fév', 'Mar', 'Avr', 'Mai', 'Juin'],
      datasets: [
        {
          label: 'Avant politique',
          data: [40, 45, 50, 48, 47, 46],
          borderColor: '#a47148', 
          fill: false
        },
        {
          label: 'Après politique',
          data: [30, 32, 28, 25, 27, 26],
          borderColor: '#25a244', 
          fill: false
        }
      ]
    };
  }

  updateRotationChart() {
    this.rotationData = {
      labels: ['Jan', 'Fév', 'Mar', 'Avr', 'Mai', 'Juin'],
      datasets: [
        {
          label: 'Taux de rotation',
          data: this.selectedCategory === 'economie' ? [0.5, 0.6, 0.55, 0.7, 0.65, 0.8] : [0.4, 0.5, 0.45, 0.6, 0.55, 0.7],
          borderColor: '#25a244', 
          fill: false
        },
        {
          label: 'Livres inutilisés',
          data: this.selectedCategory === 'economie' ? [0.1, 0.15, 0.12, 0.2, 0.18, 0.25] : [0.08, 0.12, 0.1, 0.15, 0.13, 0.2],
          borderColor: '#595959', 
          fill: false
        }
      ]
    };
    this.cdr.detectChanges(); 
  }
}