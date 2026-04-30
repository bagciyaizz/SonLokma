import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponse, LoginRequest, RegisterRequest, UserProfile } from './auth.models';

const tokenKey = 'sonlokma.accessToken';
const userKey = 'sonlokma.user';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly user = signal<UserProfile | null>(this.readUser());

  readonly currentUser = this.user.asReadonly();
  readonly isLoggedIn = computed(() => !!this.user());

  get token(): string | null {
    return localStorage.getItem(tokenKey);
  }

  login(request: LoginRequest) {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/login`, request).pipe(
      tap((response) => this.storeSession(response))
    );
  }

  register(request: RegisterRequest) {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/register`, request).pipe(
      tap((response) => this.storeSession(response))
    );
  }

  logout(): void {
    localStorage.removeItem(tokenKey);
    localStorage.removeItem(userKey);
    this.user.set(null);
    this.router.navigateByUrl('/auth/login');
  }

  private storeSession(response: AuthResponse): void {
    localStorage.setItem(tokenKey, response.accessToken);
    localStorage.setItem(userKey, JSON.stringify(response.user));
    this.user.set(response.user);
  }

  private readUser(): UserProfile | null {
    const value = localStorage.getItem(userKey);
    if (!value) {
      return null;
    }

    try {
      return JSON.parse(value) as UserProfile;
    } catch {
      localStorage.removeItem(userKey);
      return null;
    }
  }
}
