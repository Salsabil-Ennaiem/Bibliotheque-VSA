import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmpruntService {

  private apiUrl = '/api/emprunts'; // Adjust the URL as needed

  constructor(private http: HttpClient) {}

  deleteEmprunt(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // Add other methods like getEmprunts(), addEmprunt(), updateEmprunt(), etc.
}
