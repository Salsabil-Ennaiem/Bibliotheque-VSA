import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateNouveauteRequest, NouveauteDTO, NouveauteGetALL } from '../model/nouveaute.model';

@Injectable({
  providedIn: 'root'
})
export class NouveauteService {


   private apiUrl = 'http://localhost:5232/api/Nouveaute'; 


  constructor(private http: HttpClient) { }


  getAll(): Observable<NouveauteGetALL[]> {
    return this.http.get<NouveauteGetALL[]>(`${this.apiUrl}/GetallUser`);
  }

  getAllNouv(): Observable<NouveauteGetALL[]> {
    return this.http.get<NouveauteGetALL[]>(`${this.apiUrl}/Getall`);
  }
  getById(id: string): Observable<NouveauteDTO> {
    return this.http.get<NouveauteDTO>(`${this.apiUrl}/Get${id}`);
  }

  create(Nouv: CreateNouveauteRequest): Observable<NouveauteDTO> {
    return this.http.post<NouveauteDTO>(`${this.apiUrl}/Create`, Nouv);
  }

  update(id: string, Nouv: CreateNouveauteRequest): Observable<NouveauteDTO> {
    return this.http.put<NouveauteDTO>(`${this.apiUrl}/Update${id}`, Nouv);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Delete${id}`);
  }
}
