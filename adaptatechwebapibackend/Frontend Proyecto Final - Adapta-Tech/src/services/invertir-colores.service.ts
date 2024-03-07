import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class InvertirColoresService {
  private contrasteActivo = false;

  constructor() { }

  invertiColores(): void {
    this.contrasteActivo = !this.contrasteActivo;
    if (this.contrasteActivo) {
      document.body.classList.add('invertir-colores');
    } else {
      document.body.classList.remove('invertir-colores');
    }
  }
}
