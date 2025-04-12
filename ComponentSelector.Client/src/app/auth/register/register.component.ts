import { Component, inject } from '@angular/core';
import { AccountService } from '../../_services/account.service';
import { Router, RouterLink } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  accountService = inject(AccountService);
  router = inject(Router);

  registerForm: FormGroup;
  registerError: string | null = null;

  passwordVisible: boolean = false;
  confirmedPasswordVisible: boolean = false;

  constructor(private fb: FormBuilder) {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      age: ['', [Validators.required, Validators.max(120), Validators.min(1)]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      country: ['', Validators.max(256)],
      confirmedPassword: ['', Validators.required],
    });
  }

  togglePassword() {
    this.passwordVisible = !this.passwordVisible;
  }

  toggleConfirmedPassword() {
    this.confirmedPasswordVisible = !this.confirmedPasswordVisible;
  }

  register() {
    if (this.registerForm.invalid) return;

    const { username, email, age, password, confirmedPassword } =
      this.registerForm.value;

    this.accountService
      .register({ username, email, age, password, confirmedPassword })
      .subscribe({
        next: (authUser) => {
          this.accountService.setCurrentUser(authUser);
          this.router.navigate(['/']);
        },
        error: (err) => {
          this.registerError = 'Registration failed. Try again.';
          if (Array.isArray(err)) {
            this.registerError +=
              '<br>' + err.map((e) => `• ${e}`).join('<br>');
          }
        },
      });
  }
}
