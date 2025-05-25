import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Statistics } from '../_models/admin/statistics';
import { ChangeRoleRequest } from '../_models/admin/changeRoleRequest';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private http = inject(HttpClient);
  private baseUrl = environment.baseUrl;

  getStats() {
    return this.http.get<Statistics>(this.baseUrl + 'admin/stats');
  }
  changeRole(request: ChangeRoleRequest) {
    return this.http.post(this.baseUrl + 'admin/change-role', request);
  }
}
