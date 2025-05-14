import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Build } from '../_models/build/build';
import { setChatBuildHeaders } from '../_helpers/buildHelper';
import { ChatRequestQueryParams } from '../_models/queryParams/charRequestQueryParams';

@Injectable({
  providedIn: 'root',
})
export class BuildsService {
  private http = inject(HttpClient);
  private baseUrl = environment.baseUrl;

  getAiBuild(chatBuildRequest: ChatRequestQueryParams) {
    let params = setChatBuildHeaders(chatBuildRequest);
    return this.http.get<Build>(this.baseUrl + 'builds', { params });
  }
}
