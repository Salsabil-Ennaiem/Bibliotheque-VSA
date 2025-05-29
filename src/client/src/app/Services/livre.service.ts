import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateLivreRequest, LivreDTO, UpdateLivreDTO } from '../model/Livres.model';


@Injectable({
  providedIn: 'root'
})

export class LivreService {
  private apiUrl = 'http://localhost:5232/api/Livres';

  constructor(private http: HttpClient) { }

  /*search(term: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/search?term=${term}`);
  }*/


  search(term: string): Observable<LivreDTO[]> {
    return this.http.get<LivreDTO[]>(`${this.apiUrl}/search${term}`);
  }

  getAll(): Observable<LivreDTO[]> {
    return this.http.get<LivreDTO[]>(`${this.apiUrl}/GetallUser`);
  }

  getAllLiv(): Observable<LivreDTO[]> {
    return this.http.get<LivreDTO[]>(`${this.apiUrl}/Getall`);
  }
  getById(id: string): Observable<LivreDTO> {
    return this.http.get<LivreDTO>(`${this.apiUrl}/Get${id}`);
  }

  create(livre: CreateLivreRequest): Observable<LivreDTO> {
    return this.http.post<LivreDTO>(`${this.apiUrl}/Create`, livre);
  }

  update(id: string, livre: UpdateLivreDTO): Observable<LivreDTO> {
    return this.http.put<LivreDTO>(`${this.apiUrl}/Update${id}`, livre);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Delete${id}`);
  }

  import(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.apiUrl}/import`, formData);
  }

  export(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/export`, { responseType: 'blob' });
  }
}

