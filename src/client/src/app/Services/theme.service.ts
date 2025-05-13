import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private themeSubject = new BehaviorSubject<string>('light');
  theme$ = this.themeSubject.asObservable();

  constructor() {
    this.loadTheme();
  }

  private loadTheme() {
    const savedTheme = localStorage.getItem('theme') || 'light';
    this.setTheme(savedTheme);
  }

  setTheme(theme: string) {
    this.themeSubject.next(theme);
    localStorage.setItem('theme', theme);
    this.applyTheme(theme);
  }

  toggleTheme() {
    const currentTheme = this.themeSubject.value;
    const newTheme = currentTheme === 'light' ? 'dark' : 'light';
    this.setTheme(newTheme);
  }

  private applyTheme(theme: string) {
    const root = document.documentElement;
    if (theme === 'light') {
      root.style.setProperty('--primary-color', '#606C38');
      root.style.setProperty('--secondary-color', '#283618');
      root.style.setProperty('--background-color', '#FEFAE0');
      root.style.setProperty('--accent-color', '#DDA15E');
      root.style.setProperty('--highlight-color', '#BC6C25');
      root.style.setProperty('--text-color', '#283618');
    } else {
      root.style.setProperty('--primary-color', '#CCD5AE');
      root.style.setProperty('--secondary-color', '#E9EDC9');
      root.style.setProperty('--background-color', '#FEFAE0');
      root.style.setProperty('--accent-color', '#FAEDCD');
      root.style.setProperty('--highlight-color', '#D4A373');
      root.style.setProperty('--text-color', '#4A3720');
    }
  }
}