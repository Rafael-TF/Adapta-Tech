// contraste.service.ts

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ContrasteService {
  private _contrasteActivo = false;

  get contrasteActivo(): boolean {
    return this._contrasteActivo;
  }

  constructor() {}

  activarContraste(): void {
    this._contrasteActivo = true;
    document.body.classList.add('contraste-activo');
  }

  desactivarContraste(): void {
    this._contrasteActivo = false;
    document.body.classList.remove('contraste-activo');
  }

  toggleContraste(): void {
    if (this._contrasteActivo) {
      this.desactivarContraste();
    } else {
      this.activarContraste();
    }
  }
}
