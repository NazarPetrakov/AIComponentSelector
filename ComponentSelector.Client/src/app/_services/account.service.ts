import { inject, Injectable, signal } from '@angular/core';
import { AuthUser } from '../_models/authUser';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private http = inject(HttpClient);
  baseUrl = environment.baseUrl;
  currentUser = signal<AuthUser | null>(null);

  setCurrentUser(user: AuthUser) {
    const jsonUser = JSON.stringify(user);
    localStorage.setItem('authUser', jsonUser);
    this.currentUser.set(user);
  }

  login(loginData: any) {
    return this.http.post<AuthUser>(this.baseUrl + 'account/login', loginData);
  }
  register(registerData: any) {
    return this.http.post<AuthUser>(
      this.baseUrl + 'account/register',
      registerData
    );
  }
  logout() {
    localStorage.removeItem('authUser');
    this.currentUser.set(null);
  }
}
