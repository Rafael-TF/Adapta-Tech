import { Injectable } from '@angular/core';
import { environment } from '../environments/environment.development';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ICitaMedica, IModificarMedicamento } from '../interfaces/medicacion.interface';
import { Observable } from 'rxjs';
import { ITokenInfo } from '../interfaces/user.interface';
import { IAgregarCitaMedica, IModificarCitaMedica } from '../interfaces/citaMedica.interface';

@Injectable({
  providedIn: 'root'
})
export class CitaMedicaService {

  urlAPI = environment.urlAPI;
  constructor(private http: HttpClient) { }

  getCitasMedicas(idPerfilUsuario: number): Observable<ICitaMedica[]> {
    const headers = this.getHeaders();
    return this.http.get<ICitaMedica[]>(`${this.urlAPI}/CitasMedicas/${idPerfilUsuario}`, {
      headers: headers,
    });
  }

  agregarCitaMedica(citaMedica: IAgregarCitaMedica): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post<IAgregarCitaMedica>(`${this.urlAPI}/CitasMedicas/agregarCitaMedica`, citaMedica,
    {
      headers: headers,
    });
  }

  updateCitaMedica(idCita: number, citaMedica: IModificarCitaMedica): Observable<IModificarCitaMedica> {
    const headers = this.getHeaders();

    return this.http.put<IModificarCitaMedica>(
      `${this.urlAPI}/CitasMedicas/modificarCitaMedica/${idCita}`,
      citaMedica,
      {
        headers: headers,
      }
    );
  }

  deletecitaMedica(idCita: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.delete<any>(`${this.urlAPI}/CitasMedicas/eliminarCitaMedica/${idCita}`, { headers });
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