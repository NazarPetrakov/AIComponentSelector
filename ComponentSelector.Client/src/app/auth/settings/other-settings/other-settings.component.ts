import { Component, inject } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { AccountService } from '../../../_services/account.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import {
  BsModalRef,
  BsModalService,
  ModalModule,
  ModalOptions,
} from 'ngx-bootstrap/modal';
import { ConfirmModalComponent } from '../../../modals/confirm-modal/confirm-modal.component';
import { ToastrModule, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-other-settings',
  standalone: true,
  imports: [TranslateModule, CommonModule, FormsModule, ModalModule],
  templateUrl: './other-settings.component.html',
  styleUrl: './other-settings.component.css',
})
export class OtherSettingsComponent {
  private router = inject(Router);
  private toastr = inject(ToastrService);
  accountService = inject(AccountService);
  bsModalRef?: BsModalRef;

  constructor(private modalService: BsModalService) {}

  deleteAccount() {
    const options: ModalOptions = {
      initialState: {
        title: 'Confirming',
        message: 'Are you sure you want to delete your account?',
        btnOkText: 'Yes',
        btnCancelText: 'No',
      },
    };
    this.bsModalRef = this.modalService.show(ConfirmModalComponent, options);

    this.bsModalRef.onHidden?.subscribe(() => {
      if (this.bsModalRef?.content.confirmed) {
        this.accountService.deleteUser().subscribe({
          next: () => {
            this.accountService.logout();
            this.router.navigateByUrl('/');
            this.toastr.success('Your account successfully deleted');
          },
        });
      }
    });
  }
}
