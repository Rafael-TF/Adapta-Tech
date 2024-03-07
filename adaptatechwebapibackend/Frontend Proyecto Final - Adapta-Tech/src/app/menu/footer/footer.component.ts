import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CursorService } from '../../../services/cursor.service';
import { HttpClient } from '@angular/common/http';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.css'
})
export class FooterComponent implements OnInit{
  bodyCursor: string = 'auto';
  pointerCursor: string = 'pointer';
  texts: any; // propiedad para traducir los textos
  currentLanguage: string = 'es'; //almaceno el idioma actual

  constructor(private cursorService: CursorService, private http: HttpClient, private languageService: LanguageService) { }

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

  loadLanguage(language: string) {
    this.http.get(`./assets/languages/${language}.json`).subscribe((texts: any) => {
      this.texts = texts;
    });
  }
}
