import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateEmpRequest, EmppruntDTO, UpdateEmppruntDTO } from '../model/Emprunts.model';

@Injectable({
  providedIn: 'root'
})
export class EmpruntService {

  private apiUrl = 'http://localhost:5232/api/Emprunts'; 

  constructor(private http: HttpClient) {}

Notifcation(): Observable<EmppruntDTO[]> {
  return this.http.get<EmppruntDTO[]>(`${this.apiUrl}/Notification`);
  }

 search(term: string): Observable<EmppruntDTO[]> {
    return this.http.get<EmppruntDTO[]>(`${this.apiUrl}/search${term}`);
  }

  getAll(): Observable<EmppruntDTO[]> {
    return this.http.get<EmppruntDTO[]>(`${this.apiUrl}/Getall`);
  }

  getById(id: string): Observable<EmppruntDTO> {
    return this.http.get<EmppruntDTO>(`${this.apiUrl}/Get${id}`);
  }

  create(Emp: CreateEmpRequest): Observable<EmppruntDTO> {
    return this.http.post<EmppruntDTO>(`${this.apiUrl}/Create`, Emp);
  }

  update(id: string, Emp: UpdateEmppruntDTO): Observable<EmppruntDTO> {
    return this.http.put<EmppruntDTO>(`${this.apiUrl}/Update${id}`, Emp);
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
