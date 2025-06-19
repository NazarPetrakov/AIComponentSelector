import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { delay, Observable, of } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { BotMessage } from '../_models/botMessage';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private http = inject(HttpClient);
  private baseUrl = environment.baseUrl;
  messages = signal<BotMessage[] | null>(null);

  askOpenAI(message: string) {
    return this.http.post(
      this.baseUrl + 'aichat/ask',
      { message: message },
      {
        responseType: 'text',
      }
    );
  }
  getMessages() {
    return this.http.get<BotMessage[]>(this.baseUrl + 'aichat/messages');
  }
  clearChatHistory() {
    return this.http.delete(this.baseUrl + 'aichat');
  }
}
