import { Component, inject } from '@angular/core';
import { AccountService } from '../../../_services/account.service';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css',
})
export class ChangePasswordComponent {
  accountService = inject(AccountService);
  router = inject(Router);
  toastr = inject(ToastrService);

  changePasswordData = {
    currentPassword: '',
    newPassword: '',
  };

  currentPasswordVisible: boolean = false;
  newPasswordVisible: boolean = false;

  changePasswordErrors: string | null = null;

  onSubmit(form: any) {
    if (form.invalid) return;

    this.accountService.changePassword(this.changePasswordData)?.subscribe({
      next: () => {
        this.router.navigateByUrl('settings/profile');
        this.toastr.success('Password successfully changed');
      },
      error: (err) => {
        if (Array.isArray(err)) {
          this.changePasswordErrors = 'Changing password failed:';
          this.changePasswordErrors +=
            '<br>' + err.map((e) => `• ${e}`).join('<br>');
        }
      },
    });
  }

  toggleCurrentPasswordVisibility(): void {
    this.currentPasswordVisible = !this.currentPasswordVisible;
  }

  toggleNewPasswordVisibility(): void {
    this.newPasswordVisible = !this.newPasswordVisible;
  }
}
