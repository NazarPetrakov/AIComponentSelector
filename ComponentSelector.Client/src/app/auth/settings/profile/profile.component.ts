import { CommonModule, DatePipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { User } from '../../../_models/user';
import { AccountService } from '../../../_services/account.service';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [DatePipe, CommonModule, FormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css',
})
export class ProfileComponent {
  private toastr = inject(ToastrService);
  accountService = inject(AccountService);
  profileError: string | null = null;
  isUpdating = false;
  updatedUser: Partial<User> = {};
  selectedLanguage = 'UA';

  startEditing() {
    this.isUpdating = true;
    const user = this.accountService.user();
    if (user) {
      this.updatedUser = {
        userName: user.userName,
        age: user.age,
        country: user.country,
      };
    }
  }

  cancelEditing() {
    this.isUpdating = false;
    this.updatedUser = {};
  }

  saveProfile() {
    this.accountService.updateUser(this.updatedUser)?.subscribe({
      next: () => {
        if (this.accountService.user()) {
          this.accountService.user.update((user) => {
            user!.userName = this.updatedUser.userName;
            user!.age = this.updatedUser.age;
            user!.country = this.updatedUser.country;
            return user;
          });
        }
        this.toastr.success('User profile successfully updated');
        this.isUpdating = false;
      },
      error: (err) => {
        if (Array.isArray(err)) {
          this.profileError = 'Updating user profile failed:';
          this.profileError += '<br>' + err.map((e) => `• ${e}`).join('<br>');
        }
      },
    });
  }
}
