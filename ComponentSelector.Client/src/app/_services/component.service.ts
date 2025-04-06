import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { ComputerComponent } from '../_models/computerComponent';
import { PaginatedResult } from '../_models/pagination';
import { ComponentQueryParams } from '../_models/queryParams/componentQueryParams';
import {
  setPaginationHeaders,
  setPaginationResponse,
} from '../_helpers/paginationHelper';
import { of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ComponentService {
  private http = inject(HttpClient);
  private baseUrl = environment.baseUrl;
  componentQueryParams = signal<ComponentQueryParams>(
    new ComponentQueryParams()
  );
  paginatedResult = signal<PaginatedResult<ComputerComponent[]> | null>(null);
  componentsCache = new Map();

  getComponents() {
    const cacheKey = Object.keys(this.componentQueryParams())
      .map((key) => `${key}${(this.componentQueryParams() as any)[key]}`)
      .join('-');
    const cachedResponse = this.componentsCache.get(cacheKey);
    if (cachedResponse) {
      setPaginationResponse(cachedResponse, this.paginatedResult);
      return of(cachedResponse.body);
    }

    let params = setPaginationHeaders(
      this.componentQueryParams().pageNumber,
      this.componentQueryParams().pageSize
    );
    
    return this.http
      .get<ComputerComponent[]>(this.baseUrl + 'components', {
        observe: 'response',
        params,
      })
      .pipe(
        tap((response) => {
          setPaginationResponse(response, this.paginatedResult);
          this.componentsCache.set(cacheKey, response);
        })
      );
  }
}
