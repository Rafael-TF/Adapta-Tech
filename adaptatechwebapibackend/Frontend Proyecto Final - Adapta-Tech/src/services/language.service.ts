import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {
  private language = new BehaviorSubject<string>('es');

  constructor() { }

  changeLanguage(language: string) {
    this.language.next(language);
    localStorage.setItem('language', language); // Guardo el idioma en el almacenamiento local
  }

  getLanguage() {
    return this.language.asObservable();
  }
}
