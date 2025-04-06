import { Component, inject, OnInit } from '@angular/core';
import { ComponentService } from '../_services/component.service';
import { ComponentCardComponent } from '../component-card/component-card.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { timeout } from 'rxjs';

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

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      const newPage = +params['page'] || 1;

      if (this.currentPage !== newPage) {
        this.currentPage = newPage;
        this.componentsService.componentQueryParams().pageNumber = newPage;
        this.loadComponents();
      }
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
}
