import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from './nav/nav.component';
import { AccountService } from './_services/account.service';
import { AuthUser } from './_models/authUser';
import { TranslateService } from '@ngx-translate/core';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import { LoaderService } from './_services/loader.service';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavComponent, MatProgressBarModule, AsyncPipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  private accountService = inject(AccountService);
  private translateService = inject(TranslateService);
  loaderService = inject(LoaderService)

  ngOnInit(): void {
    this.setUser();
    this.setLang();
  }
  setUser() {
    const userString = localStorage.getItem('authUser');
    if (!userString) return;
    const user: AuthUser = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }
  setLang() {
    const localLanguage = localStorage.getItem('lang') || 'ua';
    localStorage.setItem('lang', localLanguage)
    this.translateService.use(localLanguage);
  }
}
