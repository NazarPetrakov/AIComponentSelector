import { Component, inject, OnInit } from '@angular/core';
import { ComponentService } from '../_services/component.service';
import { ComputerComponent } from '../_models/computerComponent';
import { ComponentCardComponent } from '../component-card/component-card.component';

@Component({
  selector: 'app-catalog',
  standalone: true,
  imports: [ComponentCardComponent],
  templateUrl: './catalog.component.html',
  styleUrl: './catalog.component.css',
})
export class CatalogComponent implements OnInit {
  private componentsService = inject(ComponentService);
  components?: ComputerComponent[];

  ngOnInit(): void {
    this.getComponents();
  }
  getComponents() {
    this.componentsService.getComponents().subscribe({
      next: (components) => (this.components = components),
    });
  }
}
