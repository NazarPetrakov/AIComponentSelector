import { inject, Injectable, signal } from '@angular/core';
import { AuthUser } from '../_models/authUser';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import { User } from '../_models/user';
import { BuildsService } from './builds.service';
import { Observable, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private http = inject(HttpClient);
  private buildsService = inject(BuildsService);
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
  setMe(): Observable<User | null> {
    if (this.currentUser()) {
      return this.http
        .get<User>(this.baseUrl + 'account/me')
        .pipe(tap((user) => this.user.set(user)));
    } else {
      return of(null);
    }
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
    this.buildsService.resetBuilds();
  }
  deleteUser() {
    return this.http.delete(this.baseUrl + 'account/me');
  }
}
