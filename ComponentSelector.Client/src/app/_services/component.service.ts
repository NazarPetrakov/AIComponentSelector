import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { ComputerComponent } from '../_models/computerComponent';

@Injectable({
  providedIn: 'root',
})
export class ComponentService {
  private http = inject(HttpClient);
  private baseUrl = environment.baseUrl;

  getComponents() {
    return this.http.get<ComputerComponent[]>(this.baseUrl + 'components');
  }
}
