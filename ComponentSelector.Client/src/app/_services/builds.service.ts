import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Build } from '../_models/build/build';
import { setChatBuildHeaders } from '../_helpers/buildHelper';
import { ChatRequestQueryParams } from '../_models/queryParams/charRequestQueryParams';
import { CreateUserBuild } from '../_models/build/createUserBuild';
import { UserBuild } from '../_models/build/userBuild';
import { BuildQueryParam } from '../_models/queryParams/buildQueryParams';
import {
  setPaginationHeaders,
  setPaginationResponse,
} from '../_helpers/paginationHelper';
import { tap } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root',
})
export class BuildsService {
  private http = inject(HttpClient);
  private baseUrl = environment.baseUrl;
  builds = signal<PaginatedResult<UserBuild[]> | null>(null);
  buildQueryParams = signal<BuildQueryParam>(new BuildQueryParam());

  constructor() {
    this.buildQueryParams().pageSize = 8;
  }

  getAiBuild(chatBuildRequest: ChatRequestQueryParams) {
    let params = setChatBuildHeaders(chatBuildRequest);
    return this.http.get<Build>(this.baseUrl + 'builds/ai-generate', {
      params,
    });
  }
  createBuild(createUserBuild: CreateUserBuild) {
    return this.http.post<number>(this.baseUrl + 'builds', createUserBuild);
  }
  deleteBuild(id: number) {
    return this.http.delete(this.baseUrl + 'builds/' + id);
  }
  setUserBuilds() {
    let params = setPaginationHeaders(
      this.buildQueryParams().pageNumber,
      this.buildQueryParams().pageSize
    );
    return this.http
      .get<UserBuild[]>(this.baseUrl + 'builds/users/me', {
        observe: 'response',
        params,
      })
      .pipe(
        tap((response) => {
          setPaginationResponse(response, this.builds);
        })
      );
  }
  resetBuilds() {
    this.builds.set(null);
  }
}
