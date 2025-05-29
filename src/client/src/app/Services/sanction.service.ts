import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateSanctionRequest, SanctionDTO } from '../model/sanction.model';

@Injectable({
  providedIn: 'root'
})
export class SanctionService {

  constructor(private http:HttpClient) { }
  private apiUrl = 'http://localhost:5232/api/Sanctions';

  search(term: string): Observable<SanctionDTO[]> {
      return this.http.get<SanctionDTO[]>(`${this.apiUrl}/search${term}`);
    }
    
      getAll(): Observable<SanctionDTO[]> {
        return this.http.get<SanctionDTO[]>(`${this.apiUrl}/Getall`);
      }
      
        create(livre: CreateSanctionRequest): Observable<SanctionDTO> {
          return this.http.post<SanctionDTO>(`${this.apiUrl}/Create`, livre);
        }
}
