import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProfileDTO, UpdateProfileDto } from '../model/bibliothecaire.model';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
 private apiUrl = 'http://localhost:5232/api/Profile';

  constructor(private http: HttpClient) { }

  get(): Observable<ProfileDTO> {
    return this.http.get<ProfileDTO>(`${this.apiUrl}/Get`);
  }

  Modifier(Profile: UpdateProfileDto): Observable<ProfileDTO> {
    return this.http.post<ProfileDTO>(`${this.apiUrl}/put`, Profile);
  }
}
