import { Component, OnInit } from '@angular/core';
import { TextoService } from '../../services/texto.service';
import { GuiaCursorService } from '../../services/guia-cursor.service';
import { HttpClient } from '@angular/common/http';
import { LanguageService } from '../../services/language.service';

@Component({
  selector: 'app-inicio',
  standalone: true,
  imports: [],
  templateUrl: './inicio.component.html',
  styleUrl: './inicio.component.css'
})
export class InicioComponent implements OnInit{
  texts: any; // propiedad para traducir los textos
  currentLanguage: string = 'es'; //almaceno el idioma actual

  constructor(private textoService: TextoService, private http: HttpClient, private languageService: LanguageService) { }

  ngOnInit(): void {
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

  obtenerTamanoTexto(): number {
    return this.textoService.getTamanoTexto();
  }

  loadLanguage(language: string) {
    this.http.get(`./assets/languages/${language}.json`).subscribe((texts: any) => {
      this.texts = texts;
    });
  }
}
