import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Bibliothecaire, IBibliothecaireModel } from '../model/bibliothecaire.model';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class BibliothecaireService {

  constructor(private http: HttpClient) { }
  
  login(obj :Bibliothecaire):Observable<IBibliothecaireModel> {
    return this.http.post<IBibliothecaireModel>("https://api.freeprojectapi.com/api/SmartParking/login", obj);
  }

  }
