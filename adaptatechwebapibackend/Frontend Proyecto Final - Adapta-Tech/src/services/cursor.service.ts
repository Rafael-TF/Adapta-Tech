import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CursorService {
  private bodyCursorSubject: BehaviorSubject<string> = new BehaviorSubject<string>('auto');
  public bodyCursor$: Observable<string> = this.bodyCursorSubject.asObservable();

  private pointerCursorSubject: BehaviorSubject<string> = new BehaviorSubject<string>('pointer');
  public pointerCursor$: Observable<string> = this.pointerCursorSubject.asObservable();

  constructor() { }

  setBodyCursor(cursor: string): void {
    this.bodyCursorSubject.next(cursor);
  }

  setPointerCursor(cursor: string): void {
    this.pointerCursorSubject.next(cursor);
  }
}
