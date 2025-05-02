import { Component, inject } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { CommonModule } from '@angular/common';
import { BuildsService } from '../_services/builds.service';
import { Build } from '../_models/build';
import { FormsModule } from '@angular/forms';
import { ComponentCardComponent } from '../component-card/component-card.component';
import { ChatRequestQueryParams } from '../_models/queryParams/charRequestQueryParams';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-ai-selecting',
  standalone: true,
  imports: [CommonModule, FormsModule, ComponentCardComponent, TranslateModule],
  templateUrl: './ai-selecting.component.html',
  styleUrl: './ai-selecting.component.css',
})
export class AiSelectingComponent {
  accountService = inject(AccountService);
  buildsService = inject(BuildsService);
  translateService = inject(TranslateService);
  chatRequestQueryParams: ChatRequestQueryParams;

  currency: 'UAH' | 'USD' = 'USD';
  build: Build | null = null;
  loading = false;
  error = '';

  purpose = 'GAMING';
  price = '';
  description = '';
  purposes = [
    'OFFICE_WORK',
    'GAMING',
    'GRAPHIC_DESIGN',
    'PROGRAMMING',
    'VIDEO_EDITING',
  ];

  constructor() {
    this.chatRequestQueryParams = new ChatRequestQueryParams();
  }

  generateBuild() {
    this.loading = true;
    this.error = '';

    let inputPrice = parseFloat(this.price);
    if (isNaN(inputPrice) || inputPrice <= 0) {
      this.error = 'Будь ласка, введіть коректну суму.';
      this.loading = false;
      return;
    }

    const priceInUsd = this.currency === 'UAH' ? inputPrice / 42 : inputPrice;

    if (priceInUsd < 100) {
      this.error = 'Мінімальний бюджет має бути не менше 100 доларів.';
      this.loading = false;
      return;
    }

    let finalPrice = this.price;
    if (this.currency === 'USD') {
      finalPrice = inputPrice.toFixed(2);
    } else {
      finalPrice = ((inputPrice * 1) / 42).toFixed(2);
    }

    this.chatRequestQueryParams.price = finalPrice;
    this.chatRequestQueryParams.purpose = this.purpose.toLowerCase();
    this.chatRequestQueryParams.lang = this.translateService.currentLang;
    this.chatRequestQueryParams.description = this.description;

    this.buildsService.getAiBuild(this.chatRequestQueryParams).subscribe({
      next: (result) => {
        this.build = result;
        console.log(result);
        this.loading = false;
      },
      error: () => {
        this.error = 'Помилка при отриманні білду. Спробуйте ще раз.';
        this.loading = false;
      },
    });
  }

  componentList() {
    if (!this.build) return [];
    return [
      { data: this.build.cpu, compatibility: this.build.compatibility.cpu },
      {
        data: this.build.motherboard,
        compatibility: this.build.compatibility.motherboard,
      },
      { data: this.build.ram, compatibility: this.build.compatibility.ram },
      { data: this.build.storage },
      { data: this.build.gpu, compatibility: this.build.compatibility.gpu },
      { data: this.build.psu, compatibility: this.build.compatibility.psu },
      { data: this.build.case, compatibility: this.build.compatibility.case },
    ];
  }
}
