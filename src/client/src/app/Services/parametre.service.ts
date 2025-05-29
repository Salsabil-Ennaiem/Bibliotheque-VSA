import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ParametreDTO } from '../model/Parametre.model';

@Injectable({
  providedIn: 'root'
})
export class ParametreService {

   private apiUrl = 'http://localhost:5232/api/Parametre';

  constructor(private http: HttpClient) { }

  getById(id: string): Observable<ParametreDTO> {
    return this.http.get<ParametreDTO>(`${this.apiUrl}/Get`);
  }

  create(Parametre: ParametreDTO): Observable<ParametreDTO> {
    return this.http.post<ParametreDTO>(`${this.apiUrl}/Update`, Parametre);
  }
}
