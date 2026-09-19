import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private base = 'http://localhost:5285/api';

  token = signal<string | null>(localStorage.getItem('rw_token'));

  login(username: string, password: string) {
    return this.http.post<{ token: string }>(`${this.base}/auth/login`, { username, password })
      .pipe(tap(res => {
        localStorage.setItem('rw_token', res.token);
        this.token.set(res.token);
      }));
  }

  logout() {
    localStorage.removeItem('rw_token');
    this.token.set(null);
  }

  isLoggedIn() {
    return !!this.token();
  }
}