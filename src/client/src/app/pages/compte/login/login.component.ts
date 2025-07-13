import { Component } from '@angular/core';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';
import { IftaLabelModule } from 'primeng/iftalabel';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../Services/auth.service';
import { LoginRequest } from '../../../model/bibliothecaire.model';



@Component({
  selector: 'app-login',
  imports: [ButtonModule,RouterLink , FormsModule  , IconFieldModule, InputIconModule ,CommonModule,IftaLabelModule,InputTextModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
 loginObj: LoginRequest = {
    email: '',
    password: ''
  };

  isLoading = false;
  errorMessage: string | null = null;

  constructor(private authService: AuthService, private router: Router) {}

  OnLogin(): void {
    this.errorMessage = null;
    if (!this.loginObj.email || !this.loginObj.password) {
      this.errorMessage = 'Veuillez saisir votre email et mot de passe.';
      return;
    }

    this.isLoading = true;
    this.authService.login(this.loginObj).subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigate(['bibliothecaire']);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Erreur lors de la connexion.';
      }
    });
  }
}
  

