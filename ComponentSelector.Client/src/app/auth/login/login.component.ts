import { Component, inject } from '@angular/core';
import { AccountService } from '../../_services/account.service';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule, RouterLink, TranslateModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  accountService = inject(AccountService);
  router = inject(Router);

  loginForm: FormGroup;
  loginError: string | null = null;

  // Password visibility control
  passwordVisible: boolean = false;

  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  togglePassword() {
    this.passwordVisible = !this.passwordVisible;
  }
  
  login() {
    if (this.loginForm.invalid) return;
    const { username, password } = this.loginForm.value;

    this.accountService.login({ username, password }).subscribe({
      next: (authUser) => {
        this.accountService.setCurrentUser(authUser);
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.loginError = 'Invalid username or password';
      },
    });
  }
}
