import { Component, inject } from '@angular/core';
import { AccountService } from '../../../_services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-change-email',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './change-email.component.html',
  styleUrl: './change-email.component.css',
})
export class ChangeEmailComponent {
  private router = inject(Router);
  private toastr = inject(ToastrService);
  accountService = inject(AccountService);

  changeEmailData = {
    newEmail: '',
  };
  changeEmailErrors: string | null = null;

  onSubmit(form: any) {
    if (form.invalid) return;

    this.accountService.changeEmail(this.changeEmailData)?.subscribe({
      next: () => {
        if (this.accountService.user()) {
          this.accountService.user.update((user) => {
            user!.email = this.changeEmailData.newEmail;
            return user;
          });
        }
        this.router.navigateByUrl('settings/profile');
        this.toastr.success('Email successfully changed');
      },
      error: (err) => {
        if (Array.isArray(err)) {
          this.changeEmailErrors = 'Changing email failed:';
          this.changeEmailErrors +=
            '<br>' + err.map((e) => `• ${e}`).join('<br>');
        }
      },
    });
  }
}
