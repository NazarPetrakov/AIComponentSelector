import { Component, inject } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-ai-selecting',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './ai-selecting.component.html',
  styleUrl: './ai-selecting.component.css'
})
export class AiSelectingComponent {
  accountService = inject(AccountService)
}
