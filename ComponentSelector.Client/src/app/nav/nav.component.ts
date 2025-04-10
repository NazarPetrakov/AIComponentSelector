import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [RouterLink, RouterModule, BsDropdownModule, FormsModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent {
  private router = inject(Router);
  accountService = inject(AccountService);
  term: string = "";

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
