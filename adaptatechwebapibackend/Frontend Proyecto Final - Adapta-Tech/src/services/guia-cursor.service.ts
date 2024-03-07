import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GuiaCursorService {
  private _activo = new BehaviorSubject<boolean>(false);
  activo$ = this._activo.asObservable();

  activar() {
    this._activo.next(true);
  }

  desactivar() {
    this._activo.next(false);
  }
  toggle() {
    this._activo.next(!this._activo.value);
  }
}
