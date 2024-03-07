import { Injectable } from "@angular/core";
import { environment } from "../environments/environment.development";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { IRegister } from "../interfaces/register.interface";
import { IMensajeForo, ITemaForo } from "../interfaces/foro.interface";
import { ITokenInfo } from "../interfaces/user.interface";

@Injectable({
  providedIn: "root",
})
export class ForoService {
  urlAPI = environment.urlAPI;

  constructor(private http: HttpClient) {}

  getTemasForo(): Observable<ITemaForo[]> {
    const headers = this.getHeaders();
    return this.http.get<ITemaForo[]>(this.urlAPI + "/TemasForo/listaTemas", {
      headers: headers,
    });
  }

  getMensajesTemaForo(idTemaForo: number): Observable<IMensajeForo[]> {
    const headers = this.getHeaders();
    return this.http.get<IMensajeForo[]>(
      this.urlAPI + `/MensajeForo/mensajesPorTema/${idTemaForo}`,
      {
        headers: headers,
      }
    );
  }

  addTemaForo(tema: ITemaForo): Observable<any> {
    const headers = this.getHeaders();

    return this.http.post<ITemaForo>(
      this.urlAPI + "/TemasForo/nuevoTema",
      tema,
      {
        headers: headers,
      }
    );
  }

  addMensajeForo(mensaje: IMensajeForo): Observable<any> {
    const headers = this.getHeaders();

    return this.http.post<IMensajeForo>(
      this.urlAPI + "/MensajeForo/",
      mensaje,
      {
        headers: headers,
      }
    );
  }

  getHeaders(): HttpHeaders {
    const usuario = JSON.parse(localStorage.getItem("usuario")!) as ITokenInfo;
    const headers = new HttpHeaders({
      Authorization: "Bearer " + usuario.token,
      "Content-Type": "application/json",
    });
    return headers;
  }
}