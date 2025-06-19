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
    // console.log('hi frim servi', this.componentQueryParams())
    const cacheKey = Object.keys(this.componentQueryParams())
      .map((key) => `${key}${(this.componentQueryParams() as any)[key]}`)
      .join('-');
    const cachedResponse = this.componentsCache.get(cacheKey);
    if (cachedResponse) {
      console.log('cachedResponse', cachedResponse);

      setPaginationResponse(cachedResponse, this.paginatedResult);
      return of(cachedResponse.body);
    }

    let params = setPaginationHeaders(
      this.componentQueryParams().pageNumber,
      this.componentQueryParams().pageSize
    );
    if (this.componentQueryParams().category) {
      params = params.append('category', this.componentQueryParams().category!);
    }
    if (this.componentQueryParams().searchTerm) {
      params = params.append(
        'searchTerm',
        this.componentQueryParams().searchTerm!
      );
    }
    if (this.componentQueryParams().orderBy) {
      params = params.append('orderBy', this.componentQueryParams().orderBy!);
    }
    if (this.componentQueryParams().orderByDesc) {
      params = params.append(
        'orderByDesc',
        this.componentQueryParams().orderByDesc!
      );
    }
    if (this.componentQueryParams().minPrice) {
      params = params.append('minPrice', this.componentQueryParams().minPrice!);
    }
    if (this.componentQueryParams().maxPrice) {
      params = params.append('maxPrice', this.componentQueryParams().maxPrice!);
    }
    if (this.componentQueryParams().availability) {
      params = params.append(
        'availability',
        this.componentQueryParams().availability!
      );
    }

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
  resetFilters() {
    this.componentQueryParams.update((params) => {
      (params.pageNumber = 1),
        (params.orderBy = undefined),
        (params.orderByDesc = undefined),
        (params.minPrice = undefined),
        (params.maxPrice = undefined),
        (params.availability = undefined);
        (params.searchTerm = undefined)
      return params;
    });
  }
}
