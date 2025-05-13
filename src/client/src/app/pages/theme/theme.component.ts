import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RippleModule } from 'primeng/ripple';
import { ThemeService } from '../Services/theme.service';

@Component({
  selector: 'app-theme',
  imports: [CommonModule, RippleModule],
  templateUrl: './theme.component.html',
  styleUrl: './theme.component.css'
})
export class ThemeComponent {
  constructor(public themeService: ThemeService) {}

}
