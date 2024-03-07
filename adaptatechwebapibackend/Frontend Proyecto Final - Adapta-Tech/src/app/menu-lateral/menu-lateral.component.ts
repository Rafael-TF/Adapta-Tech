import { Component, OnInit } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { TextoService } from '../../services/texto.service';
import { GuiaCursorService } from '../../services/guia-cursor.service';
import { ContrasteService } from '../../services/contraste.service';
import { InvertirColoresService } from '../../services/invertir-colores.service';
import { CursorService } from '../../services/cursor.service';
import { HttpClient } from '@angular/common/http';
import { LanguageService } from '../../services/language.service';

@Component({
  selector: 'app-menu-lateral',
  standalone: true,
  imports: [MatSidenavModule,MatListModule, MatIconModule, CommonModule],
  templateUrl: './menu-lateral.component.html',
  styleUrl: './menu-lateral.component.css'
})
export class MenuLateralComponent implements OnInit {
  guiaCursorActiva = false; 
  bodyCursor: string = 'auto';
  pointerCursor: string = 'pointer';
  texts: any; // propiedad para traducir los textos
  currentLanguage: string = 'es'; //almaceno el idioma actual
  
  constructor(private textoService: TextoService, private guiaCursorService: GuiaCursorService, private contrasteService: ContrasteService, private invertirColoresService: InvertirColoresService, private cursorService: CursorService, private http: HttpClient, private languageService: LanguageService) { }

  ngOnInit(): void {
    this.cursorService.bodyCursor$.subscribe(cursor => {
      this.bodyCursor = cursor;
    });
    this.cursorService.pointerCursor$.subscribe(cursor => {
      this.pointerCursor = cursor;
    });
    const storedLanguage = localStorage.getItem('language'); // Obtengo el idioma del almacenamiento local
    if (storedLanguage) {
        this.loadLanguage(storedLanguage);
    } else {
        this.loadLanguage('es'); // si no hay nada almacenado pongo el idioma por defecto
    }
    this.languageService.getLanguage().subscribe(language => {
      this.loadLanguage(language);
    });
  }

  aumentarTexto(): void {
    this.textoService.aumentarTexto();
  }

  disminuirTexto(): void {
    this.textoService.disminuirTexto();
  }
  restablecer() {
    window.location.reload();
  }
  moverGuiaCursor(): void {
    this.guiaCursorService.activar();
  }
  toggleGuiaCursor(): void {
    this.guiaCursorActiva = !this.guiaCursorActiva;
    if (this.guiaCursorActiva) {
      this.guiaCursorService.activar();
    } else {
      this.guiaCursorService.desactivar();
    }
  }
  toggleContraste(): void {
    if (this.contrasteService.contrasteActivo) {
      this.contrasteService.desactivarContraste();
    } else {
      this.contrasteService.activarContraste(); 
    }
  }
  toggleInvertirColores(): void {
    this.invertirColoresService.invertiColores();
  }
  
  cambiarCursorPersonalizado() {
    if (this.bodyCursor === 'url(../../../assets/cursor-default.png), auto') {
      // Si el cursor actual es el personalizado, restablezco el estado normal
      this.cursorService.setBodyCursor('auto');
      this.cursorService.setPointerCursor('pointer');
    } else {
      // Si el cursor actual es el normal, cambio al personalizado
      this.cursorService.setBodyCursor('url(../../../assets/cursor-default.png), auto');
      this.cursorService.setPointerCursor('url(../../../assets/cursor-mano.png), auto');
    }
  }

  loadLanguage(language: string) {
    this.http.get(`./assets/languages/${language}.json`).subscribe((texts: any) => {
      this.texts = texts;
    });
  }
  
}