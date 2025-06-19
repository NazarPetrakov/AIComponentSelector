import { Component, input } from '@angular/core';
import { ComputerComponent } from '../_models/computerComponent';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-component-card',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './component-card.component.html',
  styleUrl: './component-card.component.css',
})
export class ComponentCardComponent {
  component = input.required<ComputerComponent>();

  onImageError(event: Event): void {
    const target = event.target as HTMLImageElement;
    target.src = 'assets/images/default-featured-image.jpg';
  }
}
