import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from './nav/nav.component';
import { AccountService } from './_services/account.service';
import { AuthUser } from './_models/authUser';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  private accountService = inject(AccountService);
  ngOnInit(): void {
    this.setUser();
  }
  setUser() {
    const userString = localStorage.getItem('authUser');
    if (!userString) return;
    const user: AuthUser = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }
}
