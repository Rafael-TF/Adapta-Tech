import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs/internal/Subject';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService {
  
  private loggedIn = new Subject<boolean>();
  loggedIn$ = this.loggedIn.asObservable();

  constructor(private router: Router) { }
  
  isLoggedIn() {
    const user = localStorage.getItem('usuario');
    if (user) {
      this.loggedIn.next(true);
      return true;
    }

    this.loggedIn.next(false);
    this.router.navigate(['login']);
    return false;

  }
}