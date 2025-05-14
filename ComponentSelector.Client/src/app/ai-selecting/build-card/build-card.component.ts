import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Compatibility } from '../../_models/build/compatibility';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-build-card',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './build-card.component.html',
  styleUrl: './build-card.component.css',
})
export class BuildCardComponent {
  @Input() title!: string;
  @Input() price!: number;
  @Input() characteristics: {
    attributeName: string;
    attributeValue: string;
  }[] = [];
  @Input() compatibility!: string
}
