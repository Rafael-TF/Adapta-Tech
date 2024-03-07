import { Component } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { RouterModule, Router } from "@angular/router";
import { ForoService } from "../../../services/foro.service";
import { IMensajeForo, ITemaForo } from "../../../interfaces/foro.interface";
import { TableTemaComponent } from "./table-tema/table-tema.component";
import { LoginService } from "../../../services/login.service";
import { IPerfil } from "../../../interfaces/perfil.interface";
import { CommonModule, NgFor } from "@angular/common";
import { AuthGuardService } from "../../../guards/auth-guard.service";

@Component({
  selector: "app-foro",
  standalone: true,
  imports: [RouterModule, FormsModule, TableTemaComponent, NgFor, CommonModule],
  templateUrl: "./foro.component.html",
  styleUrl: "./foro.component.css",
})
export class ForoComponent {
  nuevoTema: ITemaForo = {
    idTemaUsuario: 0,
    titulo: "",
    fechaCreacion: null,
  };
  nuevoMensaje: IMensajeForo = {
    idUsuariomensaje: 0,
    idPerfilUsuariomensaje: 0,
    idTema: 0,
    texto: "",
    fechaMensaje: null,
  };

  nombreNuevoTema: string = "";
  textoNuevoMensaje: string = "";

  temaForoSeleccionado: number = 0;
  mostrarTodosLosTemas: boolean = true;
  mostrarTablaTema: boolean = false;
  mostrarBotonMensaje: boolean = false;
  mostrarBotonTema: boolean = true;

  tituloTemaSeleccionado: string = '';
  
  perfil!: IPerfil;

  temasForo: ITemaForo[] = [];
  mensajesForo: IMensajeForo[] = [];

  constructor(private foroService: ForoService, private router: Router, private authGuardService: AuthGuardService) {
    this.mostrarTodosLosTemas = true;
  }

  ngOnInit() {
    //cambios para no acceder al foro si no estás logueado
    if (!this.authGuardService.isLoggedIn()) {
      this.router.navigate(['login']);
    } else {
      this.perfil = JSON.parse(localStorage.getItem("perfil")!) as IPerfil;
      this.verTemas();
    }
  }

  cambiaTema(idTema: number, tituloTema: string): void {
    this.foroService.getMensajesTemaForo(idTema).subscribe({
      next: (data) => {
        this.mensajesForo = data;
        this.temaForoSeleccionado = idTema;
        this.tituloTemaSeleccionado = tituloTema;
        this.mostrarTodosLosTemas = false;
        this.mostrarTablaTema = true; 
        this.mostrarBotonMensaje = true;
        this.mostrarBotonTema = false;
      },
      error: (err) => alert("No se pudieron cargar los temas del foro."),
      complete: () => console.log("ok"),
    });
  }

  verTemas() {
    this.foroService.getTemasForo().subscribe({
      next: (data) => {
        console.log(data);
        this.temasForo = data;
      },
      error: (err) => alert("No se pudieron cargar los temas del foro."),
      complete: () => console.log("ok"),
    });
  }

  verMensajes() {
    this.foroService.getMensajesTemaForo(this.temaForoSeleccionado).subscribe({
      next: (data) => {
        console.log(data);
        this.mensajesForo = data;
      },
      error: (err) => alert("No se pudieron cargar los mensajes del tema."),
      complete: () => console.log("ok"),
    });
  }

  addTema() {
    this.nuevoTema = {
      idTemaUsuario: this.perfil.usuarioId,
      titulo: this.nombreNuevoTema,
      fechaCreacion: new Date(),
    };

    this.foroService.addTemaForo(this.nuevoTema).subscribe({
      next: (data) => {
        this.verTemas();
      },
      error: (err) => alert("No se pudo añadir el tema nuevo al foro."),
      complete: () => console.log("ok"),
    });
  }

  addMensaje() {
    this.nuevoMensaje = {
      idUsuariomensaje: this.perfil.usuarioId,
      idPerfilUsuariomensaje: this.perfil.idPerfil,
      idTema: this.temaForoSeleccionado,
      texto: this.textoNuevoMensaje,
      fechaMensaje: new Date(),
    };

    this.foroService.addMensajeForo(this.nuevoMensaje).subscribe({
      next: (data) => {
        this.verMensajes();
      },
      error: (err) =>
        alert("No se pudo añadir el mensaje nuevo al foro."),
      complete: () => console.log("ok"),
    });
  }

  getProfileFromSession() {}

  getClaseTema(index: number): string {
    return index % 2 === 0 ? 'tema-par' : 'tema-impar';
  }

  volverMostrarTodosLosTemas(): void {
    this.mostrarTablaTema = false;
    this.mostrarBotonMensaje = false;
    this.mostrarBotonTema = true;
  }
}
