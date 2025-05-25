import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

   private apiUrl = 'http://localhost:5232/api';

  constructor(private http: HttpClient) {}

  login(email: string, password: string) {
    return this.http.post<{ token: string }>(
      `${this.apiUrl}/auth/login`, 
      { email, password }
    );
  }

  forgotPassword(email: string) {
    return this.http.post<{ token: string; email: string }>(
      `${this.apiUrl}/auth/forgot-password`,
      { email }
    );
  }

  resetPassword(email: string, token: string, newPassword: string) {
    return this.http.post(
      `${this.apiUrl}/auth/reset-password`,
      { email, token, newPassword }
    );
  }
}