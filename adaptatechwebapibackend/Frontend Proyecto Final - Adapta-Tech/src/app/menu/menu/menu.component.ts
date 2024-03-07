import { Component, OnInit, OnDestroy } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CursorService } from '../../../services/cursor.service';
import { AuthGuardService } from '../../../guards/auth-guard.service';
import { CommonModule } from '@angular/common';
import { LoginService } from '../../../services/login.service';
import { IPerfil } from '../../../interfaces/perfil.interface';
import { Subscription } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { LanguageService } from '../../../services/language.service';

@Component({
    selector: 'app-menu',
    standalone: true,
    templateUrl: './menu.component.html',
    styleUrl: './menu.component.css',
    imports: [RouterModule, CommonModule]
})
export class MenuComponent implements OnInit{
  perfil: IPerfil | null = null;
  perfilSubscription: Subscription = new Subscription;
  bodyCursor: string = 'auto';
  pointerCursor: string = 'pointer';
  estaLogueado: boolean = false;
  texts: any; // propiedad para traducir los textos
  currentLanguage: string = 'es'; //almaceno el idioma actual
  
  constructor(private cursorService: CursorService, private authGuardService: AuthGuardService, private loginService: LoginService, private http: HttpClient, private languageService: LanguageService) { 
    this.perfil = JSON.parse(localStorage.getItem("perfil")!) as IPerfil;
    this.authGuardService.loggedIn$.subscribe(loggedIn => {
      this.estaLogueado = loggedIn;
    });
  }

  ngOnInit(): void {
    this.cursorService.bodyCursor$.subscribe(cursor => {
      this.bodyCursor = cursor;
    });
    this.cursorService.pointerCursor$.subscribe(cursor => {
      this.pointerCursor = cursor;
    });
    this.perfilSubscription = this.loginService.perfilActualizado$.subscribe(perfil => {
      this.perfil = perfil;
    });
    // Carga los textos en el idioma predeterminado
    this.changeLanguage('es');
  }

  ngOnDestroy(): void {
    // Desuscribirse del evento al destruir el componente para evitar fugas de memoria
    this.perfilSubscription.unsubscribe();
  }

  desloguearse(){
    this.loginService.logout();
  }

  changeLanguage(language: string) {
    this.languageService.changeLanguage(language);
    this.currentLanguage = language; //le estoy diciendo que me almace el idioma al que le haga clic
    this.http.get(`./assets/languages/${language}.json`).subscribe((texts: any) => {
      this.texts = texts;
    });
  }
}

