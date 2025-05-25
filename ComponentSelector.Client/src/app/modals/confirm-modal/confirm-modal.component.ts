import { Component, inject } from '@angular/core';
import { ModalModule, BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
@Component({
  selector: 'app-confirm-modal',
  standalone: true,
  imports: [ModalModule],
  templateUrl: './confirm-modal.component.html',
  styleUrl: './confirm-modal.component.css',
})
export class ConfirmModalComponent {
  bsModalRef = inject(BsModalRef);
  title = '';
  message = '';
  btnOkText = '';
  btnCancelText = '';
  confirmed?: boolean = false;

  confirm(): void {
    this.confirmed = true;
    this.bsModalRef?.hide();
  }

  decline(): void {
    this.bsModalRef?.hide();
  }
}
