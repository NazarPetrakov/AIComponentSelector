import { Component, input } from '@angular/core';
import { ComputerComponent } from '../_models/computerComponent';

@Component({
  selector: 'app-component-card',
  standalone: true,
  imports: [],
  templateUrl: './component-card.component.html',
  styleUrl: './component-card.component.css',
})
export class ComponentCardComponent {
  component = input.required<ComputerComponent>();
}
