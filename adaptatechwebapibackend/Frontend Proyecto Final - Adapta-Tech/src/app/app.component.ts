import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { MenuComponent } from "./menu/menu/menu.component";
import { FooterComponent } from './menu/footer/footer.component';
import { ColaboradoresComponent } from './colaboradores/colaboradores.component';
import { MenuLateralComponent } from './menu-lateral/menu-lateral.component';
import { GuiaCursorService } from '../services/guia-cursor.service';
import { Subscription } from 'rxjs';
import { CursorService } from '../services/cursor.service';
import { LanguageService } from '../services/language.service';
declare function aceptarCookies(): void;

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    imports: [CommonModule, RouterOutlet, MenuComponent, FooterComponent, ColaboradoresComponent, MenuLateralComponent]
})
export class AppComponent implements OnInit{
  cursorPosition = { x: 0, y: 0 };
  subscription: Subscription = new Subscription;
  bodyCursor: string = 'auto';
  pointerCursor: string = 'pointer';
  cuadroVisible: boolean = true; 

  constructor(public guiaCursorService: GuiaCursorService, public cursorService: CursorService, private languageService: LanguageService) {}

  ngOnInit(): void {
    this.subscription = this.guiaCursorService.activo$.subscribe(activo => {
      if (activo) {
        document.addEventListener('mousemove', this.moverGuiaCursor);
      } else {
        document.removeEventListener('mousemove', this.moverGuiaCursor);
      } 
    });
    this.cursorService.bodyCursor$.subscribe(cursor => {
      this.bodyCursor = cursor;
    });
    this.cursorService.pointerCursor$.subscribe(cursor => {
      this.pointerCursor = cursor;
    });
    // Comprueba si el usuario ya ha aceptado las cookies
    const cookiesAceptadas = sessionStorage.getItem('cookiesAceptadas');
    if (cookiesAceptadas === 'true') {
      // Si el usuario ya ha aceptado las cookies, oculta el contenedor de cookies
      this.cuadroVisible = false;
    }
    const storedLanguage = localStorage.getItem('language');
    if (storedLanguage) {
      this.languageService.changeLanguage(storedLanguage);
    } else {
      this.languageService.changeLanguage('es');
    }
  }
  
  aceptarCookies() {
    sessionStorage.setItem('cookiesAceptadas', 'true');
    const galletitasElement = document.querySelector('.galletitas') as HTMLElement;
    const contenedorCookiesElement = document.querySelector('.contenedor-cookies') as HTMLElement;

    if (galletitasElement && contenedorCookiesElement) {
      galletitasElement.style.display = 'none';
      contenedorCookiesElement.parentNode?.removeChild(contenedorCookiesElement);
    }
  }
  isMenuOpen = false;

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }
  
  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  toggleGuiaCursor(): void {
    this.guiaCursorService.toggle(); 
  }

  moverGuiaCursor(event: MouseEvent): void {
    const cursorLine = document.getElementById('cursor-line');
    if (cursorLine) {
      cursorLine.style.top = (event.clientY - 10) + 'px';
      cursorLine.style.left = (event.clientX - 250) + 'px';;
    }
  }
}