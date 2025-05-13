import { Component , inject} from '@angular/core';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import {  FormsModule } from '@angular/forms';
import { IftaLabelModule } from 'primeng/iftalabel';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { Router, RouterLink } from '@angular/router';
import { Bibliothecaire, IBibliothecaireModel } from '../../../model/bibliothecaire.model';
import { BibliothecaireService } from '../../../Services/bibliothecaire.service';



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
    loginObj:Bibliothecaire = new Bibliothecaire();

   OnLogin() {
console.log("login ok");
  }
  
}
