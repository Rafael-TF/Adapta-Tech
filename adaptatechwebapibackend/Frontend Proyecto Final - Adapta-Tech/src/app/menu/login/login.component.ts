import { Component, NgModule } from "@angular/core";
import { Router, RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { IUser } from "../../../interfaces/user.interface";
import { LoginService } from "../../../services/login.service";
import { HttpClient } from "@angular/common/http";
import { LanguageService } from "../../../services/language.service";

@Component({
  selector: "app-login",
  standalone: true,
  imports: [RouterModule, FormsModule],
  templateUrl: "./login.component.html",
  styleUrl: "./login.component.css",
})
export class LoginComponent {
  usuario: IUser = {
    email: "",
    password: "",
  };

  email: string = "";
  texts: any; // propiedad para traducir los textos
  currentLanguage: string = 'es'; //almaceno el idioma actual
  
  constructor(private loginService: LoginService, private router: Router, private http: HttpClient, private languageService: LanguageService) {}

  ngOnInit() {
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

  login() {
    this.loginService.login(this.usuario).subscribe({
      next: (data) => {
        localStorage.setItem("usuario", JSON.stringify(data));
        this.router.navigateByUrl("logueado");
      },
      error: (err) => {
        alert("Credenciales erróneas");
      },
      complete: () => {},
    });
  }
  loadLanguage(language: string) {
    this.http.get(`./assets/languages/${language}.json`).subscribe((texts: any) => {
      this.texts = texts;
    });
  }
}
