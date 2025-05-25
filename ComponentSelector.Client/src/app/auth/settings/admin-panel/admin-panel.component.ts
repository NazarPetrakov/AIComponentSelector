import { Component, inject, OnInit } from '@angular/core';
import { Statistics } from '../../../_models/admin/statistics';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AdminService } from '../../../_services/admin.service';
import { ChangeRoleRequest } from '../../../_models/admin/changeRoleRequest';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../../../_services/account.service';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-admin-panel',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, TranslateModule],
  templateUrl: './admin-panel.component.html',
  styleUrl: './admin-panel.component.css',
})
export class AdminPanelComponent implements OnInit {
  private adminService = inject(AdminService);
  private accountService = inject(AccountService);
  private fb = inject(FormBuilder);
  private toastr = inject(ToastrService);
  stats!: Statistics;
  roleForm!: FormGroup;
  roleChanged: boolean = false;

  ngOnInit(): void {
    if (!this.accountService.user()) {
    }
    this.loadStats();
    this.roleForm = this.fb.group({
      userName: ['', Validators.required],
      newRole: ['', Validators.required],
    });
  }
  loadStats() {
    this.adminService.getStats().subscribe({
      next: (data) => (this.stats = data),
    });
  }
  changeRole() {
    const request: ChangeRoleRequest = this.roleForm.value;

    this.adminService.changeRole(request).subscribe({
      next: () => {
        this.roleChanged = true;
        this.roleForm.reset({ newRole: '' });
        this.toastr.success(
          `The user '${request.userName}' is now ${request.newRole}.`
        );
      },
    });
  }
}
