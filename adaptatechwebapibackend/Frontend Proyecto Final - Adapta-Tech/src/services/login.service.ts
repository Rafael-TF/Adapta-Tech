import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, Subject, tap } from "rxjs";
import { ITokenInfo, IUser } from "../interfaces/user.interface";
import { environment } from "../environments/environment.development";
import { IPerfil, IPerfilUsuarioPut } from "../interfaces/perfil.interface";
import { Router } from "@angular/router";
import { AuthGuardService } from "../guards/auth-guard.service";

@Injectable({
  providedIn: "root",
})
export class LoginService {
  urlAPI = environment.urlAPI;
  perfilActualizado$ = new Subject<IPerfil>(); // Evento para notificar la actualización del perfil

  private sessionActive = false; // Bandera para controlar si la sesión está activa
  private sessionExpireTime = 0; // Tiempo de expiración de la sesión

  constructor(private http: HttpClient, private router: Router, private authGuardService: AuthGuardService) {
    // Verificar si hay un usuario almacenado en el almacenamiento local
    const usuarioString = localStorage.getItem("usuario") || null;
    if (usuarioString) {
      const usuario = JSON.parse(usuarioString);
      if (usuario) {
        this.sessionActive = true;
        // Establecer el tiempo de expiración de la sesión
        const expireTimeString = localStorage.getItem("sessionExpireTime") || "0";
        this.sessionExpireTime = parseInt(expireTimeString);
      }
    }

    /// Agregar un listener para el evento de recarga de la página
    window.addEventListener('unload', () => {
      if (!this.sessionActive || this.sessionExpireTime <= Date.now()) {
        // Si la sesión no debe mantenerse activa, borrar la bandera de mantener la sesión
        localStorage.removeItem("sessionExpireTime");
      }
      // Marcar la sesión como cerrada al recargar la página
      this.sessionActive = false;
      this.sessionExpireTime = 0;
    });
  }

  //Métodos para funcionamiento de la web

  login(usuario: IUser): Observable<ITokenInfo> {
    return this.http.post<ITokenInfo>(this.urlAPI + "/Auth/login", usuario);
  }

  getProfile(email: string): Observable<IPerfil> {
    const headers = this.getHeaders();
    return this.http.get<IPerfil>(
      this.urlAPI + `/PerfilUsuario/poremail/${email}`,
      { headers: headers }
    ).pipe(
      tap((perfil: IPerfil) => {
        this.perfilActualizado$.next(perfil); // Emitir evento de actualización del perfil
      })
    );
  }

  getProfileById(id: number): Observable<IPerfil> {
    const headers = this.getHeaders();
    return this.http.get<IPerfil>(
      this.urlAPI + `/PerfilUsuario/perfilPorId/${id}`,
      { headers: headers }
    );
  }

  updatePerfilUsuario(id: number, perfilUsuario: IPerfilUsuarioPut): Observable<IPerfilUsuarioPut> {
    const headers = this.getHeaders();

    return this.http.put<IPerfilUsuarioPut>(
      `${this.urlAPI}/PerfilUsuario/cambiarDatosPerfil/${id}`,
      perfilUsuario,
      {
        headers: headers,
      }
    );
  }

  deletePerfilUsuario(idPerfil: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.delete<any>(`${this.urlAPI}/PerfilUsuario/borrarPerfil/${idPerfil}`, { headers });
  }


  // Métodos para el funcionamiento del código fuente

  getHeaders(): HttpHeaders {
    const usuario = JSON.parse(localStorage.getItem("usuario")!) as ITokenInfo;
    const headers = new HttpHeaders({
      Authorization: "Bearer " + usuario.token,
      "Content-Type": "application/json",
    });
    return headers;
  }

  logout(): void {
    this.sessionActive = false;
    this.sessionExpireTime = 0;
    localStorage.removeItem("usuario");
    localStorage.removeItem("sessionExpireTime");
    this.authGuardService.isLoggedIn() == false;
    this.router.navigate(['login']);
  }

  // Método para indicar que se debe mantener la sesión activa durante cierto tiempo
  keepSessionActive(expireTime: number): void {
    this.sessionActive = true;
    // Calcular el tiempo de expiración de la sesión
    this.sessionExpireTime = Date.now() + expireTime;
    localStorage.setItem("sessionExpireTime", this.sessionExpireTime.toString());
  }
}
