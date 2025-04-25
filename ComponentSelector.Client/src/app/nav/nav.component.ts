import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { LangService } from '../_services/lang.service';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [
    RouterLink,
    RouterModule,
    BsDropdownModule,
    FormsModule,
    TranslateModule,
    BsDropdownModule,
  ],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent {
  private router = inject(Router);
  langService = inject(LangService);

  accountService = inject(AccountService);
  term: string = '';

  constructor(private translate: TranslateService) {
    this.langService.currentLang.set(localStorage.getItem('lang') || 'ua');
  }

  switchLanguage(lang: string): void {
    this.translate.use(lang);
    this.langService.currentLang.set(lang);
  }
  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/login');
  }
  onSearch() {
    if (!this.term.trim()) return;
    this.router.navigate(['/catalog/search'], {
      queryParams: { searchTerm: this.term },
    });
  }
}
