import { inject, Injectable, signal } from '@angular/core';
import { AuthUser } from '../_models/authUser';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private http = inject(HttpClient);
  baseUrl = environment.baseUrl;
  currentUser = signal<AuthUser | null>(null);
  user = signal<User | null>(null);

  setCurrentUser(user: AuthUser) {
    const jsonUser = JSON.stringify(user);
    localStorage.setItem('authUser', jsonUser);
    this.currentUser.set(user);
  }
  updateUser(updateUser: any) {
    if (this.currentUser()) {
      return this.http.put(this.baseUrl + 'account/update-user', updateUser);
    }
    return;
  }
  changeEmail(changeEmail: any) {
    if (this.currentUser()) {
      return this.http.post(this.baseUrl + 'account/change-email', changeEmail);
    }
    return;
  }
  changePassword(changePassword: any) {
    if (this.currentUser()) {
      return this.http.post(
        this.baseUrl + 'account/change-password',
        changePassword
      );
    }
    return;
  }
  getMe() {
    if (this.currentUser()) {
      return this.http.get<User>(this.baseUrl + 'account/me').subscribe({
        next: (user) => this.user.set(user),
      });
    }
    console.log('no current user found');
    return;
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
