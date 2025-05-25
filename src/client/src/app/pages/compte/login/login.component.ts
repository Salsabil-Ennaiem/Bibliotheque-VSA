import { Component , inject} from '@angular/core';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import {  FormControl, FormGroup, FormsModule, Validators } from '@angular/forms';
import { IftaLabelModule } from 'primeng/iftalabel';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../../Services/auth.service';



@Component({
  selector: 'app-login',
  imports: [ButtonModule,RouterLink , FormsModule  , IconFieldModule, InputIconModule ,CommonModule,IftaLabelModule,InputTextModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
 /* loginObj:Bibliothecaire = new Bibliothecaire();
  biblioServ =inject(BibliothecaireService);
  router=inject(Router);

  OnLogin() {
    this.biblioServ.login(this.loginObj).subscribe((res:IBibliothecaireModel) => {
      alert("user ok");
      localStorage.setItem("user",JSON.stringify(res))
      this.router.navigateByUrl('/bibliothecaire/');
    
    },
      error=>{
        alert("user not ok");})
    }*/
 
  loginObj = { emailId: '', password: '' };
  isLoading = false;
  errorMessage = '';

  constructor(private authService: AuthService) {}

  OnLogin() {
    this.isLoading = true;
    this.authService.login(this.loginObj.emailId, this.loginObj.password)
      .subscribe({
        error: (err: unknown) => {
          this.errorMessage = this.getErrorMessage(err);
          this.isLoading = false;
        }      });
  }

  private getErrorMessage(err: any): string {
    if (err.status === 401) return 'Email/mot de passe incorrect';
    if (err.status === 423) return 'Compte verrouillé';
    return 'Erreur serveur';
  }
 loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(8)])
  });

}
  

