import { Component, inject, OnInit, Query } from '@angular/core';
import { ComponentService } from '../_services/component.service';
import { ComponentCardComponent } from '../component-card/component-card.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ComponentQueryParams } from '../_models/queryParams/componentQueryParams';
import { combineLatest, map } from 'rxjs';

@Component({
  selector: 'app-catalog',
  standalone: true,
  imports: [ComponentCardComponent, PaginationModule, FormsModule],
  templateUrl: './catalog.component.html',
  styleUrl: './catalog.component.css',
})
export class CatalogComponent implements OnInit {
  componentsService = inject(ComponentService);
  router = inject(Router);
  route = inject(ActivatedRoute);

  isLoading = false;
  currentPage = 0;
  filters: ComponentQueryParams;
  sortOption: string = 'default';

  constructor() {
    this.filters = new ComponentQueryParams();
  }

  ngOnInit(): void {
    console.log('init');
    combineLatest([this.route.params, this.route.queryParams])
      .pipe(
        map(([params, queryParams]) => {
          return {
            category: params['category'],
            searchTerm: queryParams['searchTerm'],
            page: +queryParams['page'] || 1,
            minPrice: queryParams['minPrice']
              ? +queryParams['minPrice']
              : undefined,
            maxPrice: queryParams['maxPrice']
              ? +queryParams['maxPrice']
              : undefined,
            orderBy: queryParams['orderBy'] ?? undefined,
            orderByDesc: queryParams['orderByDesc'],
            availability: queryParams['availability'],
          };
        })
      )
      .subscribe((data) => {
        console.log('init data', data);
        this.currentPage = data.page;

        const currentCategory =
          this.componentsService.componentQueryParams().category;

        if (!data.category) {
          data.category = undefined;
        }

        if (data.category !== currentCategory) {
          this.sortOption = 'default';
          data.orderBy = undefined;
          data.orderByDesc = undefined;
        }

        this.componentsService.componentQueryParams.update((queryParams) => {
          queryParams.category = data.category;
          queryParams.pageNumber = data.page;
          queryParams.searchTerm = data.searchTerm;
          queryParams.minPrice = data.minPrice;
          queryParams.maxPrice = data.maxPrice;
          queryParams.orderBy = data.orderBy;
          queryParams.orderByDesc = data.orderByDesc;
          queryParams.availability = data.availability;
          return queryParams;
        });
        this.filters = this.componentsService.componentQueryParams();
        this.loadComponents();
      });
  }
  loadComponents() {
    this.isLoading = true;
    this.componentsService.getComponents().subscribe({
      next: () => {
        this.isLoading = false;
        window.scrollTo({ top: 0, behavior: 'instant' });
      },
      error: () => {
        this.isLoading = false;
      },
    });
  }
  pageChanged(event: any) {
    if (this.currentPage !== event.page) {
      this.currentPage = event.page;

      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: {
          page: event.page,
        },
        queryParamsHandling: 'merge',
        replaceUrl: true,
      });

      this.componentsService.componentQueryParams().pageNumber = event.page;
      this.loadComponents();
    }
  }
  onSortChange(): void {
    if (this.sortOption === 'price') {
      this.filters.orderBy = 'price';
      this.filters.orderByDesc = undefined;
    } else if (this.sortOption === 'priceDesc') {
      this.filters.orderBy = undefined;
      this.filters.orderByDesc = 'price';
    } else if (this.sortOption === 'reviews') {
      this.filters.orderBy = 'reviews';
      this.filters.orderByDesc = undefined;
    } else if (this.sortOption === 'reviewsDesc') {
      this.filters.orderBy = undefined;
      this.filters.orderByDesc = 'reviews';
    } else {
      this.filters.orderBy = 'id';
      this.filters.orderByDesc = undefined;
    }
  }
  applyFilters() {
    this.filters.pageNumber = 1;
    this.currentPage = 1;

    if (this.filters.availability === false)
      this.filters.availability = undefined;

    this.componentsService.componentQueryParams.update(() => {
      return this.filters;
    });

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        minPrice: this.filters.minPrice,
        maxPrice: this.filters.maxPrice,
        orderBy: this.filters.orderBy,
        orderByDesc: this.filters.orderByDesc,
        availability: this.filters.availability,
        page: this.currentPage,
      },
      queryParamsHandling: 'merge',
      replaceUrl: true,
    });

    // this.loadComponents();
  }
  resetFilters() {
    this.sortOption = 'default';
    this.filters = new ComponentQueryParams();
    this.currentPage = 1;

    this.componentsService.resetFilters();
    const category = this.route.snapshot.params['category'];

    if (!category) {
      this.router.navigate(['']);
    } else {
      this.router.navigate(['catalog', category]);
    }
  }
}
