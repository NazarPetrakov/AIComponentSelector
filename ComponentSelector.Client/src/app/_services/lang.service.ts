import { inject, Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LangService {
  currentLang = signal<string>('ua');
}
