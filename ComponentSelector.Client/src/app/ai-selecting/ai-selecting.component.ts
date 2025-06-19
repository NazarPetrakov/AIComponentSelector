import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { CommonModule } from '@angular/common';
import { BuildsService } from '../_services/builds.service';
import { Build } from '../_models/build/build';
import { FormsModule } from '@angular/forms';
import { ComponentCardComponent } from '../component-card/component-card.component';
import { ChatRequestQueryParams } from '../_models/queryParams/charRequestQueryParams';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { ChatService } from '../_services/chat.service';
import { BotMessage } from '../_models/botMessage';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BuildCardComponent } from './build-card/build-card.component';
import { ComputerComponent } from '../_models/computerComponent';
import { CreateUserBuild } from '../_models/build/createUserBuild';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-ai-selecting',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ComponentCardComponent,
    TranslateModule,
    TooltipModule,
    TranslateModule,
    BuildCardComponent,
  ],
  templateUrl: './ai-selecting.component.html',
  styleUrl: './ai-selecting.component.css',
})
export class AiSelectingComponent {
  @ViewChild('chatBody') chatBody!: ElementRef;
  private toastr = inject(ToastrService);
  chatService = inject(ChatService);
  accountService = inject(AccountService);
  buildsService = inject(BuildsService);
  translateService = inject(TranslateService);
  chatRequestQueryParams: ChatRequestQueryParams;

  saveBuildDisableToggle: boolean = false;

  currency: 'UAH' | 'USD' = 'USD';
  build: Build | null = null;
  loading = false;
  error = '';

  purpose = 'GAMING';
  price = '';
  description = '';
  purposes = [
    'OFFICE_WORK',
    'GAMING',
    'GRAPHIC_DESIGN',
    'PROGRAMMING',
    'VIDEO_EDITING',
  ];

  chatOpen = false;
  userMessage = '';
  dialogLoading = false;

  constructor() {
    this.chatRequestQueryParams = new ChatRequestQueryParams();
  }

  sendMessage() {
    if (!this.userMessage.trim()) return;

    const input = this.userMessage;

    const userMessage: BotMessage = { role: 'user', message: input };

    this.chatService.messages.update((messages) => {
      return [...(messages ?? []), userMessage];
    });

    this.userMessage = '';
    this.dialogLoading = true;

    setTimeout(() => this.scrollToBottom(), 0);

    this.chatService.askOpenAI(input).subscribe({
      next: (response) => {
        const assistantMessage: BotMessage = {
          role: 'assistant',
          message: response,
        };
        this.chatService.messages.update((messages) => {
          return [...(messages ?? []), assistantMessage];
        });
        this.dialogLoading = false;

        setTimeout(() => this.scrollToBottom(), 0);
      },
      error: (error) => {
        const wrongMessage: BotMessage = {
          role: 'assistant',
          message: 'Something went wrong',
        };
        this.chatService.messages.update((messages) => {
          return [...(messages ?? []), wrongMessage];
        });

        this.dialogLoading = false;

        setTimeout(() => this.scrollToBottom(), 0);

        console.log(error);
      },
    });
  }
  clearChatHistory() {
    this.chatService.clearChatHistory().subscribe({
      next: () =>
        this.chatService.messages.update(() => {
          return null;
        }),
    });
  }

  generateBuild() {
    this.loading = true;
    this.error = '';

    let inputPrice = parseFloat(this.price);
    if (isNaN(inputPrice) || inputPrice <= 0) {
      this.error = 'Please enter the correct budget.';
      this.loading = false;
      return;
    }

    const priceInUsd = this.currency === 'UAH' ? inputPrice / 42 : inputPrice;

    if (priceInUsd < 200) {
      this.error = 'The minimum budget must be at least $200.';
      this.loading = false;
      return;
    }

    let finalPrice = this.price;
    if (this.currency === 'USD') {
      finalPrice = inputPrice.toFixed(2);
    } else {
      finalPrice = ((inputPrice * 1) / 42).toFixed(2);
    }

    this.chatRequestQueryParams.price = finalPrice;
    this.chatRequestQueryParams.purpose = this.purpose.toLowerCase();
    this.chatRequestQueryParams.lang = this.translateService.currentLang;
    this.chatRequestQueryParams.description = this.description;

    this.buildsService.getAiBuild(this.chatRequestQueryParams).subscribe({
      next: (result) => {
        this.build = result;
        this.loading = false;
      },
      error: () => {
        this.error = 'Build error. Please try again.';
        this.loading = false;
      },
    });
  }

  addBuild() {
    if (!this.build) return;

    let createBuild: CreateUserBuild = {
      CPUId: this.build.cpu?.id ?? null,
      MotherboardId: this.build.motherboard?.id ?? null,
      RAMId: this.build.ram?.id ?? null,
      StorageId: this.build.storage?.id ?? null,
      GPUId: this.build.gpu?.id ?? null,
      PSUId: this.build.psu?.id ?? null,
      CaseId: this.build.case?.id ?? null,
    };

    console.log(createBuild);

    this.buildsService.createBuild(createBuild).subscribe({
      next: (id) => {
        this.saveBuildDisableToggle = !this.saveBuildDisableToggle;
        this.buildsService.setUserBuilds().subscribe();
        this.toastr.success(`Successful saving the build #${id}`);
      },
    });
  }

  componentList() {
    if (!this.build) return [];
    return [
      {
        category: 'cpu',
        data: this.build.cpu,
        compatibility: this.build.compatibility.cpu,
      },
      {
        category: 'motherboard',
        data: this.build.motherboard,
        compatibility: this.build.compatibility.motherboard,
      },
      {
        category: 'ram',
        data: this.build.ram,
        compatibility: this.build.compatibility.ram,
      },
      {
        category: 'gpu',
        data: this.build.gpu,
        compatibility: this.build.compatibility.gpu,
      },
      {
        category: 'gpu',
        data: this.build.psu,
        compatibility: this.build.compatibility.psu,
      },
      {
        category: 'case',
        data: this.build.case,
        compatibility: this.build.compatibility.case,
      },
      { category: 'storage', data: this.build.storage },
    ];
  }

  getDefaultComponent(category: string): ComputerComponent {
    return {
      id: -1,
      category,
      title: 'Not found',
      price: 0,
      availability: 'N/A',
      link: 'default-featured-image.jpg',
      imageUrl: 'assets/images/default-featured-image.jpg',
      reviews: 0,
    };
  }

  toggleChat() {
    if (!this.chatOpen) {
      if (!this.chatService.messages()) {
        this.chatService.getMessages().subscribe({
          next: (messages) => {
            this.chatService.messages.set(messages.reverse());

            setTimeout(() => this.scrollToBottom(), 0);
          },
        });
      } else {
        setTimeout(() => this.scrollToBottom(), 0);
      }
    }

    this.chatOpen = !this.chatOpen;
  }
  scrollToBottom() {
    try {
      this.chatBody.nativeElement.scrollTop =
        this.chatBody.nativeElement.scrollHeight;
    } catch (err) {
      console.error('Scroll failed', err);
    }
  }
}
