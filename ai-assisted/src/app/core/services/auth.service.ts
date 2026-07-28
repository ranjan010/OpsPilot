import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly storageKey = 'ai-assisted-auth';
  private readonly baseUrl = 'https://localhost:58958/api/auth';

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<boolean> {
    return this.http.post<any>(`${this.baseUrl}/login`, { email, password }).pipe(
      map(response => {
        if (response && response.accessToken) {
          localStorage.setItem(this.storageKey, 'true');
          localStorage.setItem('auth-token', response.accessToken);
          return true;
        }
        
        this.clearStorage();
        return false;
      }),
      catchError(error => {
        console.error('Login failed', error);
        this.clearStorage();
        return of(false);
      })
    );
  }

  register(email: string, password: string, firstName: string, lastName: string): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/register`, { email, password, firstName, lastName });
  }

  logout(): void {
    this.clearStorage();
  }

  get isAuthenticated(): boolean {
    return localStorage.getItem(this.storageKey) === 'true';
  }

  private clearStorage(): void {
    localStorage.removeItem(this.storageKey);
    localStorage.removeItem('auth-token');
  }
}