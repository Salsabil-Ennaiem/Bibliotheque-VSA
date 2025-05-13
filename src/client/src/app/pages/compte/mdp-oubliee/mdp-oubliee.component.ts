import { Component , OnInit} from '@angular/core';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { FormControl,FormGroup, ReactiveFormsModule } from '@angular/forms';
import { IftaLabelModule } from 'primeng/iftalabel';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-mdp-oubliee',
  imports: [RouterLink ,ButtonModule,ReactiveFormsModule  , IconFieldModule , InputIconModule ,CommonModule,IftaLabelModule,InputTextModule],
  templateUrl: './mdp-oubliee.component.html',
  styleUrl: './mdp-oubliee.component.css'
})
export class MdpOublieeComponent implements OnInit {
    email: any;
    password: any;
    formGroup: FormGroup | any;
  
  
    ngOnInit() {
      this.formGroup = new FormGroup({
        email: new FormControl(''),
        password: new FormControl(''),
      });
}
  }
